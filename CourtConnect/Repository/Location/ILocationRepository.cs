using CourtConnect.ViewModel.Club;
using CourtConnect.ViewModel.Location;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CourtConnect.Repository.Location
{
    public interface ILocationRepository
    {
        IEnumerable<SelectListItem> GetLocationForDDL();
        Task<bool> Create(LocationViewModel locationViewModel);
        Task<bool> Edit(LocationViewModel locationViewModel);
        IQueryable<LocationForDisplayViewModel> GetAllLocations();
        Task<bool> Delete(int id);
        Task<LocationViewModel> GetLocationById(int id);
    }
}
