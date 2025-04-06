
using CourtConnect.Models;
using CourtConnect.Repository.Ranking;
using CourtConnect.ViewModel.Ranking;

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

        public Task<List<RankingForDisplayViewModel>> GetRanking()
        {
            return _rankingRepository.GetRanking();
        }

        public Task<List<RankingForDisplayViewModel>> GetRankingByClubId(List<RankingForDisplayViewModel> rankingViewModels, int clubId)
        {
            return _rankingRepository.GetRankingByClubId(rankingViewModels, clubId);
        }

        public Task<List<RankingForDisplayViewModel>> GetRankingByLevelId(List<RankingForDisplayViewModel> rankingViewModels, int levelId)
        {
            return _rankingRepository.GetRankingByLevelId(rankingViewModels, levelId);
        }

        public Task<List<RankingForDisplayViewModel>> GetRankingByName(List<RankingForDisplayViewModel> rankingViewModels, string name)
        {
            return _rankingRepository.GetRankingByName(rankingViewModels, name);
        }

        public Task UpdatePlayerLevel(string userId)
        {
            return _rankingRepository.UpdatePlayerLevel(userId);
        }

        public  Task UpdatePoints(string userId, int points)
        {
            return  _rankingRepository.UpdatePoints(userId, points);
        }

    }
}
