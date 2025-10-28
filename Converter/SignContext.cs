

using System.Collections;
using System.Text;

namespace PDFUABox.ConverterServices;

public class SignContext
{
    public SignContext(string PfxData, string PfxPassword)
    {
        _pfxData = PfxData;
        this.PfxPassword = PfxPassword;
    }
    private readonly string _pfxData;

    public string PfxPassword { get; private set; }

    internal Stream GetPfxStream()
    {
        byte[] byteArray = Convert.FromBase64String(_pfxData);
        return new MemoryStream(byteArray);
    }

}
