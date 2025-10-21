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
        using TextReader reader = new StreamReader(validatedFile);
        using XmlReader xmlReader = XmlReader.Create(reader, settings);
        Compliance? compliance = (Compliance?)serializer.Deserialize(xmlReader);
        return compliance;
    }
}

