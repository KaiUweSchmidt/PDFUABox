using System.Xml.Serialization;

namespace Converter;

public static class ComplianceReportSerializer
{
    public static Compliance? Deserialize(String validatedFile)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Compliance));
        using TextReader reader = new StreamReader(validatedFile);
        Compliance? compliance = (Compliance?)serializer.Deserialize(reader);
        return compliance;
    }
}

