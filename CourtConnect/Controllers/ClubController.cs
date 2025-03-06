using CourtConnect.Service.Club;
using CourtConnect.Service.Status;
using CourtConnect.ViewModel.Club;
using Microsoft.AspNetCore.Mvc;

namespace CourtConnect.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubService _clubService;


        public ClubController(IClubService clubService)
        {
            _clubService = clubService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<ClubForDisplayViewModel> clubs = _clubService.GetAllClubs().ToList();
            return View(clubs);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult>Create(ClubViewModel clubViewModel)
        {
            bool ok = await _clubService.Create(clubViewModel);
            if (ok)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                TempData["NotificationMessage"] = "Acest club exista deja";
                TempData["NotificationType"] = "error";
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ClubViewModel clubViewModel = await _clubService.GetClubById(id);
            return View(clubViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ClubViewModel clubViewModel)
        {
            bool ok =await _clubService.Edit(clubViewModel);

            if (ok)
            {
                TempData["NotificationMessage"] = "Clubul a fost editat cu succes";
                TempData["NotificationType"] = "success";
                return RedirectToAction("Index");
            }else
            {
                return View(clubViewModel); 
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            bool ok = await _clubService.Delete(id);
            if(ok)
            {
                return RedirectToAction("Index");
            }else
            {
                return RedirectToAction("Index");
            }
        }


    }
}
//Cand iti schimbi clubul din profil sa se decrementeze numarul de jucatori din baza de date