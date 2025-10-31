using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PDFUABox.WebApp.Areas.Identity.Data;

// Add profile data for application users by adding properties to the PDFUABoxUser class
public class PDFUABoxUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Certificate { get; set; }
    public string CertificatePassword { get; set; }

    public PDFUABoxUser()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        Certificate = string.Empty;
        CertificatePassword = string.Empty;
    }

}

