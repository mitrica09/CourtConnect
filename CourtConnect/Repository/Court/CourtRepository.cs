using CourtConnect.Models;
using CourtConnect.StartPackage.Database;
using CourtConnect.ViewModel.Club;
using CourtConnect.ViewModel.Court;
using CourtConnect.ViewModel.Location;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Data.Entity;
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
            using (var transaction = _db.Database.BeginTransaction())
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

        public async Task<bool> Delete(int id)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    Models.Court court = await _db.Courts.FindAsync(id);
                    if (court == null)
                        return false;

                    _db.Courts.Remove(court);
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

        public async Task<bool> Edit(CourtViewModel courtViewModel)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    Models.Court court = await _db.Courts.FindAsync(courtViewModel.Id);
                    if (court == null)
                        return false;

                    court.Name = courtViewModel.Name;
                    court.LocationId = courtViewModel.LocationId;

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

        public IQueryable<CourtForDisplayViewModel> GetAllCourts()
        {
            return _db.Courts.Include(c=>c.Location).Select(c => new CourtForDisplayViewModel
            {

                Name = c.Name,
                Location = c.Location.City,
                Id = c.Id
            }).AsQueryable();
        }

        public async Task<CourtViewModel> GetCourtById(int id)
        {
            var court = await _db.Courts.FindAsync(id);
            if (court == null) return null;

            return new CourtViewModel
            {
                Id = court.Id,
                Name = court.Name,
                LocationId = court.LocationId,
            };
        }

        public IEnumerable<SelectListItem> GetCourtsForDDL()
        {
            List<SelectListItem> Courts = new List<SelectListItem>();
            IEnumerable<Models.Court> ListCourts = _db.Courts;
            foreach (var court in ListCourts)
            {
                Courts.Add(new SelectListItem
                {
                    Value = court.Id.ToString(),
                    Text = court.Name,
                });

            }
            return Courts;
        }
    }
}

