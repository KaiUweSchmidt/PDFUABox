using Microsoft.Extensions.Configuration;

// Fix für IDE0044 und S2933: Feld als readonly deklarieren
// Fix für S4487: Feld entfernen, da es nicht verwendet wird

// Entferne die private IConfiguration _configuration; Zeile komplett
// Passe den Konstruktor an, indem die Konfiguration nur lokal verwendet wird

namespace Converter;

public class Watcher : IDisposable
{
    private readonly string sourceDirectory;

    public string SourceDirectory { get => sourceDirectory; }

    private FileSystemWatcher? fileSystemWatcher;
    public Watcher(IConfiguration configuration)
    {
        if (configuration == null)
            throw new ArgumentNullException(nameof(configuration), "No configuration");
        this.sourceDirectory = configuration["PDFUABOX_SRC"] ??
            throw new ArgumentException("No source directory for watcher", nameof(configuration));
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
        string value = $"Created: {e.FullPath}";
        Console.WriteLine(value);
    }

    private static void OnDeleted(object sender, FileSystemEventArgs e) =>
        Console.WriteLine($"Deleted: {e.FullPath}");

    private static void OnRenamed(object sender, RenamedEventArgs e)
    {
#pragma warning disable CA1303 // Literale nicht als lokalisierte Parameter übergeben
        Console.WriteLine($"Renamed:");
#pragma warning restore CA1303 // Literale nicht als lokalisierte Parameter übergeben
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
#pragma warning disable CA1303 // Literale nicht als lokalisierte Parameter übergeben
            Console.WriteLine("Stacktrace:");
#pragma warning restore CA1303 // Literale nicht als lokalisierte Parameter übergeben
            Console.WriteLine(ex.StackTrace);
            Console.WriteLine();
            PrintException(ex.InnerException);
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    private bool isDisposed;
    protected virtual void Dispose(bool disposing)
    {
        if (isDisposed) return;

        if (disposing &&fileSystemWatcher != null)
            fileSystemWatcher.Dispose();

        isDisposed = true;
    }
}
