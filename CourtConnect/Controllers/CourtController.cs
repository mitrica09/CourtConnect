using CourtConnect.Repository.Court;
using CourtConnect.Repository.Location;
using CourtConnect.ViewModel.Court;
using CourtConnect.ViewModel.Location;
using Microsoft.AspNetCore.Mvc;

namespace CourtConnect.Controllers
{
    public class CourtController : Controller
    {
       private readonly ILocationRepository _locationRepository;

        private readonly ICourtRepository _courtRepository;

        public CourtController(ILocationRepository locationRepository
                             , ICourtRepository courtRepository)
        {
            _locationRepository = locationRepository;
            _courtRepository = courtRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            CourtViewModel courtViewModel = new CourtViewModel(_locationRepository);            
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

    }
}
