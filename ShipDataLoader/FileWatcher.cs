using ShipDataLoader.Contracts;

namespace ShipDataLoader
{
    public class FileWatcher
    {
        private readonly FileSystemWatcher _watcher;
        private readonly IFileProcessor _fileProcessor;

        public FileWatcher(string folderPath, IFileProcessor fileProcessor)
        {
            _watcher = new FileSystemWatcher(folderPath);
            _fileProcessor = fileProcessor;
            _watcher.Created += OnNewFile;
        }

        public void Start()
        {
            _watcher.EnableRaisingEvents = true;
        }

        private async void OnNewFile(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"New file detected: {e.FullPath}");
            await _fileProcessor.ProcessFileAsync(e.FullPath);
        }
    }

}
