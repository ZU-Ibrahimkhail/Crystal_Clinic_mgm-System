using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Crystal_Clinic_Mgm.UI.Controllers
{
    [AllowAnonymous]
    public class FallbackController :
        Microsoft.AspNetCore.Mvc.Controller
    {
        public IActionResult Index()
        {
            //string subfolderPath = "ReactFiles"; // Specify the subfolder path here
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html");
            return PhysicalFile(filePath, "text/HTML");
        }
    }
}