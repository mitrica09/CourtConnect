using CourtConnect.Repository.Court;
using CourtConnect.Service.Court;
using CourtConnect.Repository.Location;
using CourtConnect.ViewModel.Club;
using CourtConnect.ViewModel.Court;
using CourtConnect.ViewModel.Location;
using Microsoft.AspNetCore.Mvc;

namespace CourtConnect.Controllers
{
    public class CourtController : Controller
    {
       private readonly ILocationRepository _locationRepository;

        private readonly ICourtRepository _courtRepository;
        private readonly ICourtService _courtService;
        public CourtController(ILocationRepository locationRepository
                             , ICourtRepository courtRepository
                             ,ICourtService courtService)
        {
            _locationRepository = locationRepository;
            _courtRepository = courtRepository;
            _courtService = courtService;

        }

        [HttpGet]
        public IActionResult Index()
        {
            List<CourtForDisplayViewModel> court = _courtService.GetAllCourts().ToList();
            return View(court);
        }

        [HttpGet]
        public IActionResult Create()
        {
            CourtViewModel courtViewModel = new CourtViewModel(_locationRepository);            
            return View(courtViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            CourtViewModel courtViewModel = await _courtService.GetCourtById(id);
            courtViewModel.Locations = _locationRepository.GetLocationForDDL();

            return View(courtViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Create(CourtViewModel courtViewModel)
        {
            bool ok = await _courtRepository.Create(courtViewModel);
            if (ok)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                CourtViewModel courtViewModel2 = new CourtViewModel(_locationRepository);
                return View(courtViewModel2);
                
            }

        }

        [HttpPost]
        public async Task<IActionResult> Edit(CourtViewModel courtViewModel)
        {
            bool ok = await _courtService.Edit(courtViewModel);

            if (ok)
            {
                TempData["NotificationMessage"] = "Clubul a fost editat cu succes";
                TempData["NotificationType"] = "success";
                return RedirectToAction("Index");
            }
            else
            {
                return View(courtViewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            bool ok = await _courtService.Delete(id);
            if (ok)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}
