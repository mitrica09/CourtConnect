using CourtConnect.ViewModel.Match;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CourtConnect.Service.Match
{
    public interface IMatchService
    {
        Task<MatchDetailsViewModel> GetMatchDetails(int announceId, int matchId);
        public IEnumerable<SelectListItem> GetSetsForDDL();
        public IEnumerable<SelectListItem> GetPlayersForDDL(int matchId);
        public IEnumerable<SelectListItem> GetScoresForDDL();
        public Task<bool> CreateResultMatch(MatchResultViewModel model);
        public MatchResultViewModel GetScoreForDDL(int matchId);
        public Task<bool> SetScoreAlreadyExists(int matchId, int setId);
        public Task<Models.Match> GetMatchById(int matchId);
    }
}
