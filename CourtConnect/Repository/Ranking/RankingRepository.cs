
using CourtConnect.Models;
using CourtConnect.StartPackage.Database;
using CourtConnect.ViewModel.Ranking;
using Microsoft.AspNetCore.Identity;
using System.Data.Entity;
using System.Net;

namespace CourtConnect.Repository.Ranking
{
    public class RankingRepository : IRankingRepository
    {
        
        private readonly CourtConnectDbContext _db;
        private readonly UserManager<User> _userManager;
 
        public RankingRepository(CourtConnectDbContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }



        public async Task<int> GetPointsByUserId(string userId)
        {
            return _db.Rankings
                             .Where(r => r.UserId == userId)
                             .Select(s => s.Points)
                             .FirstOrDefault();
        }

        public async Task<List<RankingForDisplayViewModel>> GetRanking()
        {
            List<RankingForDisplayViewModel> rankingViewModels = new List<RankingForDisplayViewModel>();
            var userRanking = _db.Rankings.OrderByDescending(r => r.Points).ToList();
            foreach (var rank in userRanking)
            {
                var user = await _userManager.FindByIdAsync(rank.UserId);
                user.Club = await _db.Clubs.FindAsync(user.ClubId);
                user.Level = await _db.Levels.FindAsync(user.LevelId);

                RankingForDisplayViewModel rankingViewModel = new RankingForDisplayViewModel();

                rankingViewModel.Points = rank.Points;
                rankingViewModel.LevelId = user.LevelId;
                rankingViewModel.Level = user.Level.Name;
                rankingViewModel.ClubId = user.ClubId;
                rankingViewModel.Club = user.Club.Name;
                rankingViewModel.FullName = user.FullName;
                rankingViewModels.Add(rankingViewModel);
            }

            return rankingViewModels;
        }

        public async  Task<List<RankingForDisplayViewModel>> GetRankingByClubId(List<RankingForDisplayViewModel> rankingViewModels, int clubId)
        {
            return rankingViewModels.Where(r => r.ClubId == clubId).ToList();
        }

        public async Task<List<RankingForDisplayViewModel>> GetRankingByLevelId(List<RankingForDisplayViewModel> rankingViewModels, int levelId)
        {
           return rankingViewModels.Where(r=>r.LevelId == levelId).ToList();
        }

        public async  Task<List<RankingForDisplayViewModel>> GetRankingByName(List<RankingForDisplayViewModel> rankingViewModels, string name)
        {
            return rankingViewModels.Where(s => s.FullName.ToUpper().Contains(name.ToUpper())).ToList();
        }

        public async Task UpdatePoints(string userId, int points)
        {
            try
            {
                var userRanking = _db.Rankings
                    .FirstOrDefault(r => r.UserId == userId); // Sincron

                if (userRanking != null)
                {
                    userRanking.Points = points;
                    await _db.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("User not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in UpdatePoints method: {ex.Message}", ex);
            }
        }
    }
}


