using CourtConnect.Service.Level;
using CourtConnect.Service.Location;
using CourtConnect.ViewModel.Level;
using CourtConnect.ViewModel.Location;
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
        [HttpGet]
        public IActionResult Index()
        {
            List<LevelForDisplayViewModel> levels = _levelService.GetAllLevels().ToList();
            return View(levels);
        }
        [HttpGet]
        public IActionResult Create() 
        {
            return View();
        }
        public async Task<IActionResult> Edit(int id)
        {
            LevelViewModel levelViewModel = await _levelService.GetLevelById(id);
            return View(levelViewModel);
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

        [HttpPost]
        public async Task<IActionResult> Edit(LevelViewModel levelViewModel)
        {
            bool ok = await _levelService.Edit(levelViewModel);

            if (ok)
            {
                TempData["NotificationMessage"] = "Nivelul a fost editat cu succes";
                TempData["NotificationType"] = "success";
                return RedirectToAction("Index");
            }
            else
            {
                return View(levelViewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            bool ok = await _levelService.Delete(id);
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
