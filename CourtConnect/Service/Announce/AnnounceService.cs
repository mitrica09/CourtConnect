using CourtConnect.Repository.Announce;
using CourtConnect.ViewModel.Announce;
using CourtConnect.ViewModel.Court;

namespace CourtConnect.Service.Announce
{
    public class AnnounceService : IAnnounceService
    {
        private readonly IAnnounceRepository _announceRepository;
        

        public AnnounceService(IAnnounceRepository announceRepository)
        {
            _announceRepository = announceRepository;
        }

        public async Task<bool> ConfirmGuest(int announceId, string userId)
        {
            return await _announceRepository.ConfirmGuest(announceId, userId);
        }

        public async Task<bool> ConfirmHost(int announceId, string userId)
        {
            return await _announceRepository.ConfirmHost(announceId, userId);
        }
        public async Task<bool> RejectGuest(int announceId, string userId)
        {
            return await _announceRepository.RejectGuest(announceId, userId);
        }

        public async Task<bool> Create(AnnounceFormViewModel announceForm)
        {
            return await _announceRepository.Create(announceForm);
        }

        public async  Task<List<AnnounceForDisplayViewModel>> GetAllAnnounces()
        {
            return await _announceRepository.GetAllAnnounces();
        }

        public async Task<AnnounceDetailsViewModel> GetAnnounceDetails(int announceId, string userId)
        {
            return await _announceRepository.GetAnnounceDetails(announceId, userId);
        }

        public async Task<List<AnnounceDetailsViewModel>> GetMyAnnounces()
        {
            return await _announceRepository.GetMyAnnounces();
        }

        public async Task<bool> CreateMatch(int announceId)
        {
            return await _announceRepository.CreateMatch(announceId);
        }
    }
}
