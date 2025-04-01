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
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddScore(MatchResultViewModel model)
        {
            ModelState.Remove("Sets");
            ModelState.Remove("Players");
            ModelState.Remove("Scores");

            if (!ModelState.IsValid)
            {
                model = _matchService.GetScoreForDDL(model.MatchId);
                return View(model);
            }

            var result = await _matchService.CreateResultMatch(model);

            if (result)
            {
                return RedirectToAction("Details", new { announceId = 0, matchId = model.MatchId }); // ajustează dacă e nevoie
            }

            ModelState.AddModelError("", "A apărut o eroare la salvarea scorului.");
            return View(model);
        }

    }
}
