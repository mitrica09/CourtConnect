using CourtConnect.Repository.Match;
using CourtConnect.ViewModel.Match;

namespace CourtConnect.Service.Match
{
    public class MatchService : IMatchService
    { 
        private readonly IMatchRepository _matchRepository;

        public MatchService(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }

        public async Task<MatchDetailsViewModel> GetMatchDetails(int announceId, int matchId)
        {
            return await _matchRepository.GetMatchDetails(announceId, matchId);
        }
    }
}
