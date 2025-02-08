using CourtConnect.Service.Location;
using CourtConnect.ViewModel.Location;
using Microsoft.AspNetCore.Mvc;

namespace CourtConnect.Controllers
{
    public class LocationController : Controller
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
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
        public async Task<IActionResult>Create(LocationViewModel locationViewModel) 
        {
            bool ok = await _locationService.Create(locationViewModel);
            if (ok) 
            {
                return RedirectToAction("Index","Admin");
            }else
            {
                return View();
            }
           
        }
    }
}
