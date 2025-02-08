using CourtConnect.Repository.Court;
using CourtConnect.ViewModel.Court;

namespace CourtConnect.Service.Court
{
    public class CourtService : ICourtService
    {
        private readonly ICourtRepository _courtRepository;

        public CourtService(ICourtRepository courtRepository)
        {
            _courtRepository = courtRepository;
        }

        public async Task<bool> Create(CourtViewModel courtViewModel)
        {
            return await _courtRepository.Create(courtViewModel);
        }
    }


}
