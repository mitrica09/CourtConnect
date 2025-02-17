using CourtConnect.Service.Location;
using CourtConnect.ViewModel.Club;
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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<LocationForDisplayViewModel> locations = _locationService.GetAllLocations().ToList();
            return View(locations);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            LocationViewModel locationViewModel = await _locationService.GetLocationById(id);
            return View(locationViewModel);
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

        [HttpPost]
        public async Task<IActionResult> Edit(LocationViewModel locationViewModel)
        {
            bool ok = await _locationService.Edit(locationViewModel);

            if (ok)
            {
                TempData["NotificationMessage"] = "Locatia a fost editat cu succes";
                TempData["NotificationType"] = "success";
                return RedirectToAction("Index");
            }
            else
            {
                return View(locationViewModel);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            bool ok = await _locationService.Delete(id);
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
