using CourtConnect.ViewModel.Location;

namespace CourtConnect.Service.Location
{
    public interface ILocationService
    {
        Task<bool> Create(LocationViewModel locationViewModel);
        Task<bool> Edit(LocationViewModel locationViewModel);
        IQueryable<LocationForDisplayViewModel> GetAllLocations();
        Task<bool> Delete(int id);
        Task<LocationViewModel> GetLocationById(int id);
    }
}
