using Azure.Storage.Blobs;
using StackExchange.Redis;

class Program
{
    private static readonly string redisHost = "redis-14332.c273.us-east-1-2.ec2.redns.redis-cloud.com";
    private static readonly int redisPort = 14332;
    private static readonly string redisUser = "default";
    private static readonly string redisPassword = "ASbFdPamMZ5bUpfdpqxveR0LDwHScAmn";

    private static readonly string blobConnectionString = "";
    private static readonly string containerName = "movies";

    private static ConnectionMultiplexer redisConnection;
    private static BlobContainerClient blobContainerClient;
    private static HttpClient httpClient = new HttpClient();

    static async Task Main(string[] args)
    {
        var redisConfig = new ConfigurationOptions
        {
            EndPoints = { { redisHost, redisPort } },
            User = redisUser,
            Password = redisPassword,
            AbortOnConnectFail = false
        };
        redisConnection = ConnectionMultiplexer.Connect(redisConfig);
        var redisDb = redisConnection.GetDatabase();

        blobContainerClient = new BlobContainerClient(blobConnectionString, containerName);
        await blobContainerClient.CreateIfNotExistsAsync();

        Console.WriteLine("Service started. Checking Redis list for poster URLs...");

        while (true)
        {
            string redisListKey = "poster-urls";

            var posterUrl = await redisDb.ListLeftPopAsync(redisListKey);

            if (posterUrl.IsNullOrEmpty)
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
                continue;
            }

            Console.WriteLine($"Processing poster URL: {posterUrl}");

            try
            {
                var imageBytes = await httpClient.GetByteArrayAsync(posterUrl);

                var fileName = GetFileNameFromUrl(posterUrl);
                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = Guid.NewGuid().ToString() + ".jpg";
                }

                var blobClient = blobContainerClient.GetBlobClient(fileName);
                using var stream = new System.IO.MemoryStream(imageBytes);
                await blobClient.UploadAsync(stream, overwrite: true);

                Console.WriteLine($"Uploaded {fileName} to blob storage.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing URL {posterUrl}: {ex.Message}");
                await redisDb.ListRightPushAsync(redisListKey, posterUrl);
            }
        }
    }

    static string GetFileNameFromUrl(string url)
    {
        try
        {
            var uri = new Uri(url);
            return System.IO.Path.GetFileName(uri.LocalPath);
        }
        catch
        {
            return null;
        }
    }
}
