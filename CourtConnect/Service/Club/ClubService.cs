using CourtConnect.Repository.Club;
using CourtConnect.ViewModel.Club;

namespace CourtConnect.Service.Club
{
    public class ClubService : IClubService
    {
        private readonly IClubRepository _clubRepository;

        public ClubService(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
        }

        public async Task<bool> Create(ClubModelView clubModelView)
        {
            return await _clubRepository.Create(clubModelView);
        }
    }
}
