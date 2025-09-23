namespace PDFUABoxService;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        FileService fileService = new FileService();
        fileService.StartSMBServer();
    }
}
