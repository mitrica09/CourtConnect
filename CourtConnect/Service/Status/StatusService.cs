using CourtConnect.Repository.Status;
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

    }
}
