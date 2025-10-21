using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Converter.ComplianceReportModels;

public static class ComplianceReportSerializer
{
    public static Compliance? Deserialize(String validatedFile)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Compliance));
        XmlReaderSettings settings = new XmlReaderSettings
        {
            DtdProcessing = DtdProcessing.Prohibit,
            XmlResolver = null
        };
        int length = (int)new FileInfo(validatedFile).Length;
        StringBuilder stringBuilder = new StringBuilder(length);
        foreach (string line in File.ReadLines(validatedFile))
        {

            if (line.Contains("<Problems S", StringComparison.InvariantCulture))
            {
                string fixedLine = line.Replace("<Problems S", "<Problem S", StringComparison.InvariantCulture);
                fixedLine = fixedLine.Replace("</Problems>", "</Problem>", StringComparison.InvariantCulture);
                stringBuilder.AppendLine(fixedLine + Environment.NewLine);
            }
            else if (line.Contains("</Problems>", StringComparison.InvariantCulture))
            {
                string fixedLine = line.Replace("</Problems>", "</Problem>", StringComparison.InvariantCulture);
                stringBuilder.AppendLine(fixedLine + Environment.NewLine);
            }
            else
            {
                stringBuilder.AppendLine(line + Environment.NewLine);
            }
        }
        using TextReader reader = new StringReader(stringBuilder.ToString());
        using XmlReader xmlReader = XmlReader.Create(reader, settings);
        Compliance? compliance = (Compliance?)serializer.Deserialize(xmlReader);
        return compliance;
    }
}

