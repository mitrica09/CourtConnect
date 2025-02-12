using CourtConnect.ViewModel.Club;
using System.Runtime.CompilerServices;

namespace CourtConnect.Service.Club
{
    public interface IClubService
    {
        Task<bool> Create(ClubViewModel clubViewModel);

        IQueryable<ClubForDisplayViewModel> GetAllClubs();

        Task<bool> Edit(ClubViewModel clubViewModel);

        Task<bool> Delete(int id);

        Task<ClubViewModel> GetClubById(int id);
    }
}
