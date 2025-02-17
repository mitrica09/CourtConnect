using CourtConnect.ViewModel.Status;

namespace CourtConnect.Service.Status
{
    public interface IStatusService
    {
        Task<bool> Create(StatusViewModel statusViewModel);
        Task<bool> Edit(StatusViewModel statusViewModel);
        IQueryable<StatusForDisplayViewModel> GetAllStatuses();
        Task<bool> Delete(int id);
        Task<StatusViewModel> GetStatusById(int id);
    }
}
