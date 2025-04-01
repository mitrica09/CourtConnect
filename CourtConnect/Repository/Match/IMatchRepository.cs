using CourtConnect.ViewModel.Announce;
using CourtConnect.ViewModel.Match;

namespace CourtConnect.Repository.Match
{
    public interface IMatchRepository
    {
        Task<MatchDetailsViewModel> GetMatchDetails(int announceId, int matchId);
    }
}
