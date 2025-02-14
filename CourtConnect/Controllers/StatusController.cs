using CourtConnect.ViewModel.Status;
using Microsoft.AspNetCore.Mvc;
using CourtConnect.Service.Status;
using CourtConnect.ViewModel.Level;

namespace CourtConnect.Controllers
{
    public class StatusController : Controller
    {
        private readonly IStatusService _statusService;

        public StatusController(IStatusService statusService)
        {
            _statusService = statusService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<StatusForDisplayViewModel> statuses = _statusService.GetAllStatuses().ToList();
            return View(statuses);
        }

        [HttpGet]
        public IActionResult Create() 
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            StatusViewModel statusViewModel = await _statusService.GetStatusById(id);
            return View(statusViewModel);
        }

        [HttpPost]
        public async Task<IActionResult>Create(StatusViewModel statusViewModel)
        {
            bool ok = await _statusService.Create(statusViewModel);
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
        public async Task<IActionResult> Edit(StatusViewModel statusViewModel)
        {
            bool ok = await _statusService.Edit(statusViewModel);

            if (ok)
            {
                TempData["NotificationMessage"] = "Statusul a fost editat cu succes";
                TempData["NotificationType"] = "success";
                return RedirectToAction("Index");
            }
            else
            {
                return View(statusViewModel);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            bool ok = await _statusService.Delete(id);
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
