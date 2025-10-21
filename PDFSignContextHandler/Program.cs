using Converter;
namespace PDFSignContextHandler;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Bitte eine PDF-Datei als Argument übergeben.");
                return;
            }

            string pdfPath = args[0];
            if (!System.IO.File.Exists(pdfPath))
            {
                Console.WriteLine("Die angegebene Datei existiert nicht.");
                return;
            }
            Console.WriteLine($"PDF wird signiert: {pdfPath}");
            Sign.SignDocument(pdfPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler beim Signieren der PDF-Datei: {ex.Message}");
        }
    }
}