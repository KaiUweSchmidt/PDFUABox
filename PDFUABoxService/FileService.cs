using SMBLibrary;
using SMBLibrary.Authentication.GSSAPI;
using SMBLibrary.Authentication.NTLM;
using SMBLibrary.Client;
using SMBLibrary.Server;
using SMBLibrary.Win32;
using SMBLibrary.Win32.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace PDFUABoxService;

internal class FileService
{
    internal void StartSMBServer()
    { 
        // Define the shared folder path
        string sharedFolderPath = @"C:\PDFUABoxFiles";
        if (!Directory.Exists(sharedFolderPath))
        {
            Directory.CreateDirectory(sharedFolderPath);
        }

        // Create a virtual file system for the shared folder
        FileSystemShare share = new FileSystemShare("PDFUABox", new NTDirectoryFileSystem(sharedFolderPath));

        var smbShares = new SMBShareCollection();
        smbShares.Add(share);

        NTLMAuthenticationProviderBase authenticationMechanism = new IntegratedNTLMAuthenticationProvider();


        var securityProvider = new GSSProvider(authenticationMechanism);
        // Initialize the SMB server
        SMBServer server = new SMBServer(smbShares, securityProvider);
        server.Start(IPAddress.Any, SMBTransportType.DirectTCPTransport,true,true);
    }
}
