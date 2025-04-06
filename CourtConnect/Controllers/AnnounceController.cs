using CourtConnect.Service.Announce;
using CourtConnect.Service.Court;
using CourtConnect.Service.Match;
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
        
        public async Task<IActionResult> MyAnnounces()
        {
            if (User.Identity.IsAuthenticated)
            {
                List<AnnounceDetailsViewModel> announces = await _announceService.GetMyAnnounces();
                return View(announces);
            }
            else
            {
                TempData["NotificationMessage"] = "Trebuie sa fii logat";
                TempData["NotificationType"] = "error";
                return RedirectToAction("Login", "Account");
            }
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
                return RedirectToAction("MyAnnounces","Announce");
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

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            AnnounceDetailsViewModel announceDetailsViewModel =await _announceService.GetAnnounceDetails(announceId, userId);
            return View(announceDetailsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmGuest(int announceId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var success = await _announceService.ConfirmGuest(announceId, userId);

            if (success)
            {
                TempData["SuccessMessage"] = "Ai confirmat meciul!";
            }
            else
            {
                TempData["ErrorMessage"] = "Eroare la confirmarea meciului!";
            }

            return RedirectToAction("MyAnnounces");
        }


        [HttpPost]
        public async Task<IActionResult> ConfirmHost(int announceId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var success = await _announceService.ConfirmHost(announceId, userId);

            if (success)
            {
                // Obținem din nou anunțul ca să verificăm statusul actualizat
                var announce = await _announceService.GetAnnounceDetails(announceId, userId);

                if (announce.ConfirmHost && announce.ConfirmGuest)
                {
                    // Dacă amândoi au confirmat, creăm meciul și redirectăm spre detalii
                    var matchId = await _announceService.CreateMatch(announceId);

                    if (matchId != null)
                    {
                        return RedirectToAction("Details", "Match", new { announceId = announceId, matchId = matchId });
                        // 👈 te duce direct la pagina de meci
                    }
                }

                TempData["SuccessMessage"] = "Meci confirmat!";
            }
            else
            {
                TempData["ErrorMessage"] = "Eroare la confirmare!";
            }

            return RedirectToAction("MyAnnounces");
        }

        [HttpPost]
        public async Task<IActionResult> RejectGuest(int announceId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var success = await _announceService.RejectGuest(announceId, userId);

            if (success)
            {
                TempData["SuccessMessage"] = "Adversarul a fost refuzat!";
            }
            else
            {
                TempData["ErrorMessage"] = "Eroare la refuz!";
            }

            return RedirectToAction("MyAnnounces");
        }
    }
}
