using CourtConnect.Service.Level;
using CourtConnect.Service.Location;
using CourtConnect.ViewModel.Level;
using Microsoft.AspNetCore.Mvc;

namespace CourtConnect.Controllers
{
    public class LevelController : Controller
    {
        private readonly ILevelService _levelService;

        public LevelController(ILevelService levelService)
        {
            _levelService = levelService;
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
        public async Task<IActionResult>Create(LevelViewModel levelViewModel)
        {
            bool ok = await _levelService.Create(levelViewModel);
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
