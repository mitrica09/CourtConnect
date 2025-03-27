using CourtConnect.ViewModel.Match;

namespace CourtConnect.Service.Match
{
    public interface IMatchService
    {
        Task<MatchDetailsViewModel> GetMatchDetails(int announceId, int matchId);

    }
}
