using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using StackExchange.Redis;

namespace Trigger;

public class ProcessMovieQueueTrigger
{
    private static readonly Lazy<ConnectionMultiplexer> LazyConnection = new(() =>
    {
        var config = new ConfigurationOptions
        {
            EndPoints = { { "redis-14332.c273.us-east-1-2.ec2.redns.redis-cloud.com", 14332 } },
            User = "default",
            Password = "ASbFdPamMZ5bUpfdpqxveR0LDwHScAmn",
            AbortOnConnectFail = false
        };
        return ConnectionMultiplexer.Connect(config);
    });

    private static ConnectionMultiplexer Connection => LazyConnection.Value;
    private readonly IDatabase _redis;
    private readonly HttpClient _httpClient;
    private readonly string _omdbApiKey;

    public ProcessMovieQueueTrigger()
    {
        _redis = Connection.GetDatabase();
        _httpClient = new HttpClient();
        _omdbApiKey = Environment.GetEnvironmentVariable("d606021");
    }

    [Function("ProcessMovieQueueTrigger")]
    public async Task RunAsync(
        [QueueTrigger("film-queue", Connection = "AzureWebJobsStorage")] string queueMessage,
        ILogger log)
    {
        log.LogInformation($"Received queue message: {queueMessage}");

        var response = await _httpClient.GetAsync($"http://www.omdbapi.com/?t={Uri.EscapeDataString(queueMessage)}&apikey={_omdbApiKey}");
        if (!response.IsSuccessStatusCode)
        {
            log.LogError("OMDB request failed.");
            return;
        }

        var json = await response.Content.ReadAsStringAsync();
        var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        if (root.TryGetProperty("Poster", out var posterElement))
        {
            var posterUrl = posterElement.GetString();
            if (!string.IsNullOrWhiteSpace(posterUrl))
            {
                await _redis.ListRightPushAsync("poster-urls", posterUrl);
                log.LogInformation($"Poster URL cached in Redis: {posterUrl}");
            }
            else
            {
                log.LogWarning("Poster URL is empty");
            }
        }
        else
        {
            log.LogWarning("Poster field not found in OMDB response.");
        }
    }
}