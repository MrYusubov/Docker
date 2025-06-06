namespace M1_Api_Side.Services
{
    public interface IQueueService
    {
        Task SendMessageAsync(string message);
        Task SendMessageWithCountAsync(string message, int count);
        Task<string> ReceiveMessageAsync();
    }
}
