using CourtConnect.ViewModel.Announce;
using CourtConnect.ViewModel.Match;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CourtConnect.Repository.Match
{
    public interface IMatchRepository
    {
        Task<MatchDetailsViewModel> GetMatchDetails(int announceId, int matchId);
        public IEnumerable<SelectListItem> GetSetsForDDL();
        public IEnumerable<SelectListItem> GetPlayersForDDL(int matchId);
        public IEnumerable<SelectListItem> GetScoresForDDL();
        public Task<bool> CreateResultMatch(MatchResultViewModel model);
        public MatchResultViewModel GetScoreForDDL(int matchId);
        public Task<bool> SetScoreAlreadyExists(int matchId, int setId);

    }
}
