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
    }
}
