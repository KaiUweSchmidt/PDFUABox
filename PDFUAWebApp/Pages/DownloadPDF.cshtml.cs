using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PDFUABox.ConverterServices;
using PDFUABox.WebApp.Areas.Identity.Data;
using PDFUABox.WebApp.Extensions;

namespace PDFUABox.WebApp.Pages;

public class DownloadPDFModel : PageModel
{
    private readonly Converter _converter;
    private readonly UserManager<PDFUABoxUser> _userManager;
    public DownloadPDFModel(Converter converter, UserManager<PDFUABoxUser> userManager)
    {
        _converter = converter;
        _userManager = userManager;
    }
}
