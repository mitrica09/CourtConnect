
using CourtConnect.Repository.Ranking;

namespace CourtConnect.Service.Ranking
{
    public class RankingService : IRankingService
    {
        private readonly IRankingRepository _rankingRepository;

        public RankingService(IRankingRepository rankingRepository)
        {
            _rankingRepository = rankingRepository;
        }

        public Task<int> GetPointsByUserId(string userId)
        {
            return _rankingRepository.GetPointsByUserId(userId);
        }
    }
}
