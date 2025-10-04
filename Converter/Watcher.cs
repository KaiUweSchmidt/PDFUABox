using Microsoft.Extensions.Configuration;

namespace Converter;

public class Watcher
{
    private readonly string sourceDirectory;

    public string SourceDirectory { get => sourceDirectory; }

    private FileSystemWatcher? fileSystemWatcher;
    private IConfiguration _configuration;
    public Watcher(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException("No configuration", nameof(configuration));
        this.sourceDirectory = configuration["PDFUABOX_SRC"] ??
            throw new ArgumentException("No source directory for watcher", "PDFUABOX_SRC");
    }

    public void Start()
    {
        if(fileSystemWatcher != null)
        {
            throw new InvalidOperationException("Watcher is already started.");
        }

        Console.WriteLine("Engine started.");
        Console.WriteLine($"Source Directory: {sourceDirectory}");

        fileSystemWatcher = new FileSystemWatcher(sourceDirectory);
        fileSystemWatcher.NotifyFilter = NotifyFilters.Attributes
                             | NotifyFilters.CreationTime
                             | NotifyFilters.DirectoryName
                             | NotifyFilters.FileName
                             | NotifyFilters.LastAccess
                             | NotifyFilters.LastWrite
                             | NotifyFilters.Security
                             | NotifyFilters.Size;

        fileSystemWatcher.Created += OnCreated;
        fileSystemWatcher.Deleted += OnDeleted;
        fileSystemWatcher.Renamed += OnRenamed;
        fileSystemWatcher.Error += OnError;

        fileSystemWatcher.Filter = "*.docx";
        fileSystemWatcher.IncludeSubdirectories = true;
        fileSystemWatcher.EnableRaisingEvents = true;

    }

    public void Stop()
    {
        if(fileSystemWatcher == null)
        {
            throw new InvalidOperationException("Watcher is not started.");
        }
        fileSystemWatcher.EnableRaisingEvents = false;
        fileSystemWatcher.Dispose();
        fileSystemWatcher = null;
    }   

    private static void OnChanged(object sender, FileSystemEventArgs e)
    {
        if (e.ChangeType != WatcherChangeTypes.Changed)
        {
            return;
        }
        throw new NotSupportedException($"TODO: Remove the right to Edit in the fileshare :: Changed: {e.FullPath}");
        
    }

    private static void OnCreated(object sender, FileSystemEventArgs e)
    {
        //CreateJobFor(e.FullPath);
        string value = $"Created: {e.FullPath}";
        Console.WriteLine(value);
    }

    private static void OnDeleted(object sender, FileSystemEventArgs e) =>
        Console.WriteLine($"Deleted: {e.FullPath}");

    private static void OnRenamed(object sender, RenamedEventArgs e)
    {
        Console.WriteLine($"Renamed:");
        Console.WriteLine($"    Old: {e.OldFullPath}");
        Console.WriteLine($"    New: {e.FullPath}");
    }

    private static void OnError(object sender, ErrorEventArgs e) =>
        PrintException(e.GetException());

    private static void PrintException(Exception? ex)
    {
        if (ex != null)
        {
            Console.WriteLine($"Message: {ex.Message}");
            Console.WriteLine("Stacktrace:");
            Console.WriteLine(ex.StackTrace);
            Console.WriteLine();
            PrintException(ex.InnerException);
        }
    }

}
