namespace VolumeTask.Services
{
    public interface IFileLogger
    {
        Task LogInfoAsync(string message);
        Task LogErrorAsync(string message);
        Task WriteLogAsync(string path, string level, string message);
    }
}
