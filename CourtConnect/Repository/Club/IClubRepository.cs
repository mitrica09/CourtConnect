using CourtConnect.ViewModel.Club;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CourtConnect.Repository.Club
{
    public interface IClubRepository
    {
        public IEnumerable<SelectListItem> GetClubForDDL();
        Task<bool> Create(ClubViewModel clubViewModel);
        Task<bool> Edit(ClubViewModel clubViewModel);
        IQueryable<ClubForDisplayViewModel> GetAllClubs();

        Task<bool> Delete(int id);

        Task<ClubViewModel> GetClubById(int id);

    }
}