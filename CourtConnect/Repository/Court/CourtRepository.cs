using CourtConnect.Models;
using CourtConnect.StartPackage.Database;
using CourtConnect.ViewModel.Court;
using CourtConnect.ViewModel.Location;
using System.Collections.Generic;
using System.Linq;

namespace CourtConnect.Repository.Court
{
    public class CourtRepository : ICourtRepository
    {
        private readonly CourtConnectDbContext _db;

        public CourtRepository(CourtConnectDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(CourtViewModel courtViewModel)
        {
            using (var transation = _db.Database.BeginTransaction())
            {
                try
                {
                    Models.Court court = new Models.Court
                    {
                        Name = courtViewModel.Name,
                        LocationId = Convert.ToInt32(courtViewModel.LocationId)
                    };
                    _db.Courts.Add(court);
                    await _db.SaveChangesAsync();
                    transation.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transation.Rollback();
                    throw ex;
                }
            }
        }
    }
}
