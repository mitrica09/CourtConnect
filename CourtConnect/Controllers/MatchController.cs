using CourtConnect.Service.Match;
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
    }
}
