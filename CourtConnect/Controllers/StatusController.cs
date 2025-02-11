using CourtConnect.ViewModel.Status;
using Microsoft.AspNetCore.Mvc;
using CourtConnect.Service.Status;

namespace CourtConnect.Controllers
{
    public class StatusController : Controller
    {
        private readonly IStatusService _statusService;

        public StatusController(IStatusService statusService)
        {
            _statusService = statusService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create() 
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult>Create(StatusViewModel statusViewModel)
        {
            bool ok = await _statusService.Create(statusViewModel);
            if (ok)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return View();
            }
        }
    }
}
