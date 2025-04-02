using CourtConnect.Service.Match;
using CourtConnect.ViewModel.Match;
using Microsoft.AspNetCore.Mvc;

namespace CourtConnect.Controllers
{
    public class MatchController : Controller
    {
        private readonly IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Details(int announceId, int matchId)
        {
            var match = await _matchService.GetMatchDetails(announceId, matchId);
            if (match == null)
            {
                return NotFound();
            }

            return View(match); // trimitem ViewModel-ul către pagina Details.cshtml
        }

        [HttpGet]
        public async Task<IActionResult> AddScore(int matchId)
        {
            var model = _matchService.GetScoreForDDL(matchId);
            model.SetResults = new List<MatchResultItemViewModel>
            {
                new MatchResultItemViewModel()
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddScore(MatchResultViewModel model)
        {
            ModelState.Remove("Sets");
            ModelState.Remove("Players");
            ModelState.Remove("Scores");

            // verificare pentru maxim 3 seturi
            if (model.SetResults.Count > 3)
            {
                ModelState.AddModelError("", "Nu poți adăuga mai mult de 3 seturi.");
                model = _matchService.GetScoreForDDL(model.MatchId);
                return View(model);
            }

            if (!ModelState.IsValid || model.SetResults == null || !model.SetResults.Any())
            {
                model = _matchService.GetScoreForDDL(model.MatchId);
                model.SetResults = new List<MatchResultItemViewModel>();
                return View(model);
            }

            var result = await _matchService.CreateResultMatch(model); // salvezi toate seturile

            if (result)
            {
                return RedirectToAction("Details", new { announceId = 0, matchId = model.MatchId });
            }

            ModelState.AddModelError("", "A apărut o eroare la salvarea scorului.");
            return View(model);
        }





    }
}
