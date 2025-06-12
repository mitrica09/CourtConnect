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
                TempData["NotificationMessage"] = "Anuntul a fost adaugat cu succes";
                TempData["NotificationType"] = "success";
                return RedirectToAction("MyAnnounces","Announce");
            }
            else
            {
                TempData["NotificationMessage"] = "Oopps, a aparut o eroare va rugam sa contactati responsabilul de aplicatie!";
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

                TempData["NotificationMessage"] = "Ai confirmat meciul!";
                TempData["NotificationType"] = "success";
            }
            else
            {
                TempData["NotificationMessage"] = "Eroare la confirmarea meciului!";
                TempData["NotificationType"] = "error";
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
                var announce = await _announceService.GetAnnounceDetails(announceId, userId);

                if (announce.ConfirmHost && announce.ConfirmGuest)
                {
                    var matchId = await _announceService.CreateMatch(announceId);

                    if (matchId != null)
                    {
                        TempData["NotificationMessage"] = "Meci confirmat!";
                        TempData["NotificationType"] = "success";
                        return RedirectToAction("Details", "Match", new { announceId = announceId, matchId = matchId });

                    }
                }

            }
            else
            {
                TempData["NotificationMessage"] = "Eroare la confirmarea meciului!";
                TempData["NotificationType"] = "error";
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
                TempData["NotificationMessage"] = "Adversarul a fost refuzat cu succes!";
                TempData["NotificationType"] = "success";
            }
            else
            {
                TempData["NotificationMessage"] = "Eroare la refuzarea adversarului!";
                TempData["NotificationType"] = "error";
            }

            return RedirectToAction("MyAnnounces");
        }
    }
}
