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
        public async Task<IActionResult>Create(ClubModelView clubModelView)
        {
            bool ok = await _clubService.Create(clubModelView);
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
