using CourtConnect.Models;
using CourtConnect.ViewModel.Club;
using CourtConnect.ViewModel.Court;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace CourtConnect.Repository.Court
{
    public interface ICourtRepository
    {
        Task<bool> Create(CourtViewModel courtViewModel);
        Task<bool> Edit(CourtViewModel courtViewModel);
        IQueryable<CourtForDisplayViewModel> GetAllCourts();
        Task<bool> Delete(int id);
        Task<CourtViewModel> GetCourtById(int id);
        public IEnumerable<SelectListItem> GetCourtsForDDL();
    }
}
