using CourtConnect.Repository.Location;
using CourtConnect.ViewModel.Location;

namespace CourtConnect.Service.Location
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;

        public LocationService(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public Task<bool> Create(LocationViewModel locationViewModel)
        {
            return _locationRepository.Create(locationViewModel);
        }

        public Task<bool> Delete(int id)
        {
            return _locationRepository.Delete(id);
        }

        public async Task<bool> Edit(LocationViewModel locationViewModel)
        {
            return await _locationRepository.Edit(locationViewModel);
        }

        public IQueryable<LocationForDisplayViewModel> GetAllLocations()
        {
            return _locationRepository.GetAllLocations();
        }

        public Task<LocationViewModel> GetLocationById(int id)
        {
            return _locationRepository.GetLocationById(id);
        }
    }
}
