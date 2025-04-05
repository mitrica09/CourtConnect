using CourtConnect.ViewModel.Ranking;

namespace CourtConnect.Service.Ranking
{
    public interface IRankingService
    {        
        public Task<int> GetPointsByUserId(string userId);
        public Task<List<RankingForDisplayViewModel>> GetRanking();

        Task<List<RankingForDisplayViewModel>> GetRankingByLevelId(List<RankingForDisplayViewModel> rankingViewModels, int levelId);
        Task<List<RankingForDisplayViewModel>> GetRankingByClubId(List<RankingForDisplayViewModel> rankingViewModels, int clubId);
        Task<List<RankingForDisplayViewModel>> GetRankingByName(List<RankingForDisplayViewModel> rankingViewModels, string name);
        public Task UpdatePoints(string userId, int points);

    }
}
