using CourtConnect.Repository.Match;
using CourtConnect.ViewModel.Match;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace CourtConnect.Service.Match
{
    public class MatchService : IMatchService
    { 
        private readonly IMatchRepository _matchRepository;

        public MatchService(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }

        public async Task<bool> CreateResultMatch(MatchResultViewModel model)
        {
            return await _matchRepository.CreateResultMatch(model);
        }

        public async Task<MatchDetailsViewModel> GetMatchDetails(int announceId, int matchId)
        {
            return await _matchRepository.GetMatchDetails(announceId, matchId);
        }

        public IEnumerable<SelectListItem> GetPlayersForDDL(int matchId)
        {
            return _matchRepository.GetPlayersForDDL(matchId);
        }

        public IEnumerable<SelectListItem> GetScoresForDDL()
        {
            return _matchRepository.GetScoresForDDL();
        }

        public IEnumerable<SelectListItem> GetSetsForDDL()
        {
            return _matchRepository.GetSetsForDDL();
        }

        public MatchResultViewModel PrepareAddScoreViewModel(int matchId)
        {
            return _matchRepository.PrepareAddScoreViewModel(matchId);
        }
    }
}
