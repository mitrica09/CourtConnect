using CourtConnect.StartPackage.Database;
using CourtConnect.ViewModel.Level;
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

        public async Task<bool> Delete(int id)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    Models.Status status = await _db.Statuses.FindAsync(id);
                    if (status == null)
                        return false;

                    _db.Statuses.Remove(status);
                    await _db.SaveChangesAsync();
                    transaction.Commit();
                    return true;

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public async Task<bool> Edit(StatusViewModel statusViewModel)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    Models.Status status = await _db.Statuses.FindAsync(statusViewModel.Id);
                    if (status == null)
                        return false;
                    status.Name = statusViewModel.Name;
                    await _db.SaveChangesAsync();
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public IQueryable<StatusForDisplayViewModel> GetAllStatuses()
        {
            return _db.Statuses.Select(c => new StatusForDisplayViewModel
            {
                Id = c.Id,
                Name = c.Name,
            }).AsQueryable();

        }

        public async Task<StatusViewModel> GetStatusById(int id)
        {
            var status = await _db.Statuses.FindAsync(id);
            if (status == null) return null;

            return new StatusViewModel
            {
                Id = status.Id,
                Name = status.Name,

            };
        }
    }
}
