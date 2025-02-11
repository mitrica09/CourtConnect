using CourtConnect.StartPackage.Database;
using CourtConnect.ViewModel.Status;

namespace CourtConnect.Repository.Status
{
    public class StatusRepository : IStatusRepository
    {
        private readonly CourtConnectDbContext _db;

        public StatusRepository(CourtConnectDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(StatusViewModel statusViewModel) 
        {
            using (var transation = _db.Database.BeginTransaction())
            {
                try
                {
                    Models.Status status = new Models.Status
                    {
                        Name = statusViewModel.Name,
                    };
                    _db.Statuses.Add(status);
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
