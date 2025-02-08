using CourtConnect.ViewModel.Location;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CourtConnect.Repository.Location
{
    public interface ILocationRepository
    {
        Task<bool> Create(LocationViewModel locationViewModel);

         IEnumerable<SelectListItem> GetLocationForDDL();
    }
}
