using CourtConnect.ViewModel.Status;

namespace CourtConnect.Service.Status
{
    public interface IStatusService
    {
        Task<bool> Create(StatusViewModel statusViewModel);
    }
}
