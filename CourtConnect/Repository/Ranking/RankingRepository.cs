
using CourtConnect.StartPackage.Database;
using System.Data.Entity;

namespace CourtConnect.Repository.Ranking
{
    public class RankingRepository : IRankingRepository
    {
        private readonly CourtConnectDbContext _db;

        public RankingRepository(CourtConnectDbContext db)
        {
            _db = db;
        }

        public async Task<int> GetPointsByUserId(string userId)
        {
            return   _db.Rankings
                             .Where(r => r.UserId == userId)
                             .Select(s => s.Points)
                             .FirstOrDefault();
        }
    }
}
