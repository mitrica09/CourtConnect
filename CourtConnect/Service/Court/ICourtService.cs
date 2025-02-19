using CourtConnect.ViewModel.Court;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CourtConnect.Service.Court
{
    public interface ICourtService
    {
        Task<bool> Create(CourtViewModel courtViewModel);
        Task<bool> Edit(CourtViewModel courtViewModel);
        IQueryable<CourtForDisplayViewModel> GetAllCourts();

        Task<bool> Delete(int id);

        Task<CourtViewModel> GetCourtById(int id);
        public IEnumerable<SelectListItem> GetCourtsForDDL();
    }
}
