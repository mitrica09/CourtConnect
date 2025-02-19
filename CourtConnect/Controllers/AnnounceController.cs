using CourtConnect.Service.Announce;
using CourtConnect.Service.Court;
using CourtConnect.ViewModel.Announce;
using CourtConnect.ViewModel.Club;
using Microsoft.AspNetCore.Mvc;

namespace CourtConnect.Controllers
{
    public class AnnounceController : Controller
    {
        private readonly IAnnounceService _announceService;
        private readonly ICourtService _courtService;

        public AnnounceController(IAnnounceService announceService, ICourtService courtService)
        {
            _announceService = announceService;
            _courtService = courtService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            AnnounceFormViewModel announceFormView = new AnnounceFormViewModel(_courtService);
            return View(announceFormView);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AnnounceFormViewModel announceFormViewModel)
        {
            bool ok = await _announceService.Create(announceFormViewModel);
            if (ok)
            {
                TempData["NotificationMessage"] = "Anutul a fost adaugat cu succes";
                TempData["NotificationType"] = "succes";
                return RedirectToAction("Index","Announce");
            }
            else
            {
                TempData["NotificationMessage"] = "Oopps a aparut o eroare va rugam sa contactati responsabilul de aplicatie";
                TempData["NotificationType"] = "error";
                return View();
            }
        }
    }
}
