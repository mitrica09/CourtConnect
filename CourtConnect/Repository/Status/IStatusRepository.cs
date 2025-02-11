using CourtConnect.ViewModel.Status;

namespace CourtConnect.Repository.Status
{
    public interface IStatusRepository
    {
        Task<bool> Create(StatusViewModel statusViewModel);

    }
}
