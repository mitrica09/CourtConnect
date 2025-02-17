using CourtConnect.Repository.Status;
using CourtConnect.ViewModel.Level;
using CourtConnect.ViewModel.Status;

namespace CourtConnect.Service.Status
{
    public class StatusService : IStatusService
    {
        private readonly IStatusRepository _statusRepository;

        public StatusService(IStatusRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }

        public async Task<bool> Create(StatusViewModel statusViewModel)
        {
            return await _statusRepository.Create(statusViewModel);
        }

        public Task<bool> Delete(int id)
        {
            return _statusRepository.Delete(id);
        }

        public async Task<bool> Edit(StatusViewModel statusViewModel)
        {
            return await _statusRepository.Edit(statusViewModel);
        }

        public IQueryable<StatusForDisplayViewModel> GetAllStatuses()
        {
            return _statusRepository.GetAllStatuses();
        }

        public Task<StatusViewModel> GetStatusById(int id)
        {
            return _statusRepository.GetStatusById(id);
        }
    }
}
