using CourtConnect.StartPackage.Database;
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
                        County = locationViewModel.CountryName,
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
    }
}
