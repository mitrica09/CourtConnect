using CourtConnect.Service.Announce;
using CourtConnect.Service.Court;
using CourtConnect.ViewModel.Announce;
using CourtConnect.ViewModel.Club;
using CourtConnect.ViewModel.Level;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        public async Task<IActionResult> Index()
        {
           List<AnnounceForDisplayViewModel> announces = await _announceService.GetAllAnnounces();
           return View(announces);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (User.Identity.IsAuthenticated)
            {
                AnnounceFormViewModel announceFormView = new AnnounceFormViewModel(_courtService);
                return View(announceFormView);
            } else
            {
                TempData["NotificationMessage"] = "Trebuie sa fii logat pentru a adauga un anunt";
                TempData["NotificationType"] = "error";
                return RedirectToAction("Login", "Account");
            }
               
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
        public async Task<IActionResult> MatchroomDetails(int announceId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            AnnounceDetailsViewModel announceDetailsViewModel =await _announceService.GetAnnounceDetails(announceId, userId);
            return View(announceDetailsViewModel);


        }


    }
}
