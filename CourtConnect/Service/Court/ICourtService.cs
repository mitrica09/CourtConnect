using CourtConnect.ViewModel.Court;

namespace CourtConnect.Service.Court
{
    public interface ICourtService
    {
        Task<bool> Create(CourtViewModel courtViewModel);
    }
}
