using CourtConnect.ViewModel.Level;
using CourtConnect.ViewModel.Status;

namespace CourtConnect.Repository.Status
{
    public interface IStatusRepository
    {
        Task<bool> Create(StatusViewModel statusViewModel);
        Task<bool> Edit(StatusViewModel statusViewModel);
        IQueryable<StatusForDisplayViewModel> GetAllStatuses();
        Task<bool> Delete(int id);
        Task<StatusViewModel> GetStatusById(int id);
    }
}
