using CourtConnect.ViewModel.Club;

namespace CourtConnect.Service.Club
{
    public interface IClubService
    {
        Task<bool> Create(ClubModelView clubModelView);
    }
}
