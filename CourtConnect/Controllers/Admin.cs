using Microsoft.AspNetCore.Mvc;

namespace CourtConnect.Controllers
{
    public class Admin : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
