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

        public async Task<bool> Create(AnnounceFormViewModel announceForm)
        {
            return await _announceRepository.Create(announceForm);
        }

        public Task<List<AnnounceForDisplayViewModel>> GetAllAnnounces()
        {
            return _announceRepository.GetAllAnnounces();
        }
    }
}
