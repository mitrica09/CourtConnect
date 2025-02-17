using CourtConnect.StartPackage.Database;
using CourtConnect.ViewModel.Club;
using CourtConnect.ViewModel.Location;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CourtConnect.Repository.Location
{
    public class LocationRepository : ILocationRepository
    {
        private readonly CourtConnectDbContext _db;

        public LocationRepository(CourtConnectDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(LocationViewModel locationViewModel)
        {
           using (var transation = _db.Database.BeginTransaction())
            {
                try
                {
                    Models.Location location = new Models.Location
                    {
                        City = locationViewModel.CityName,
                        County = locationViewModel.CountyName,
                        Street = locationViewModel.StreetName,
                        Number = locationViewModel.StreetNumber
                    };
                    _db.Locations.Add(location);
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

        public IEnumerable<SelectListItem> GetLocationForDDL()
        {
            List<SelectListItem> Locations = new List<SelectListItem>();
            IEnumerable<Models.Location> LocationsList = _db.Locations;
            foreach (var location in LocationsList)
            {
                Locations.Add(new SelectListItem
                {
                    Value = location.Id.ToString(),
                    Text = location.City + "( " + location.Street + ", " + location.Number + ".)",

                });

            }
            return Locations;
        }

        public async Task<bool>Edit(LocationViewModel locationViewModel)
        {
            using(var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    Models.Location location = await _db.Locations.FindAsync(locationViewModel.Id);
                    if (location == null)
                        return false;
                    location.City = locationViewModel.CityName;
                    location.County = locationViewModel.CountyName;
                    location.Street = locationViewModel.StreetName;
                    location.Number = locationViewModel.StreetNumber;
                    await _db.SaveChangesAsync();
                    transaction.Commit();
                    return true;
                }catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public IQueryable<LocationForDisplayViewModel> GetAllLocations()
        {
            return _db.Locations.Select(c => new LocationForDisplayViewModel
            {
                Id = c.Id,
                CityName = c.City,
                CountyName = c.County,
                StreetName = c.Street,
                StreetNumber = c.Number,
            }).AsQueryable();

        }

        public async Task<bool> Delete(int id)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    Models.Location location = await _db.Locations.FindAsync(id);
                    if (location == null)
                        return false;

                    _db.Locations.Remove(location);
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

        public async Task<LocationViewModel> GetLocationById(int id)
        {
            var location = await _db.Locations.FindAsync(id);
            if (location == null) return null;

            return new LocationViewModel
            {
                Id = location.Id,
                CityName = location.City,
                CountyName = location.County,
                StreetName= location.Street,
                StreetNumber= location.Number,
            };
        }
    }
}
