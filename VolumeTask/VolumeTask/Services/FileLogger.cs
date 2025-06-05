namespace VolumeTask.Services
{
    public class FileLogger: IFileLogger
    {
        private readonly string _infoLogPath = Path.Combine("Info", "logs.txt");
        private readonly string _errorLogPath = Path.Combine("Error", "logs.txt");

        public async Task LogInfoAsync(string message)
        {
            await WriteLogAsync(_infoLogPath, "INFO", message);
        }

        public async Task LogErrorAsync(string message)
        {
            await WriteLogAsync(_errorLogPath, "ERROR", message);
        }

        public async Task WriteLogAsync(string path, string level, string message)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));

                using (var stream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read))
                using (var writer = new StreamWriter(stream))
                {
                    var logEntry = $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} [{level}] {message}";
                    await writer.WriteLineAsync(logEntry);
                }
            }
            catch{}
        }
    }

}
