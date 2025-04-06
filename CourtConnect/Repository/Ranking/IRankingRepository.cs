using CourtConnect.ViewModel.Ranking;

namespace CourtConnect.Repository.Ranking
{
    public interface IRankingRepository
    {
        Task<int> GetPointsByUserId(string userId);

        Task<List<RankingForDisplayViewModel>> GetRanking();

        Task<List<RankingForDisplayViewModel>> GetRankingByLevelId(List<RankingForDisplayViewModel> rankingViewModels, int levelId);
        Task<List<RankingForDisplayViewModel>> GetRankingByClubId(List<RankingForDisplayViewModel> rankingViewModels, int clubId);
        Task<List<RankingForDisplayViewModel>> GetRankingByName(List<RankingForDisplayViewModel> rankingViewModels, string name);
        public Task UpdatePoints(string userId, int points);
        public Task UpdatePlayerLevel(string userId);


    }
}
