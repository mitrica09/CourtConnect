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

        public async Task<bool> Create(ClubViewModel clubViewModel)
        {
            return await _clubRepository.Create(clubViewModel);
        }

        public Task<bool> Delete(int id)
        {
          return _clubRepository.Delete(id);
        }

        public async Task<bool> Edit(ClubViewModel clubViewModel)
        {
            return await _clubRepository.Edit(clubViewModel);
        }

        public IQueryable<ClubForDisplayViewModel> GetAllClubs()
        {
           return _clubRepository.GetAllClubs();
        }

        public Task<ClubViewModel> GetClubById(int id)
        {
            return _clubRepository.GetClubById(id);
        }
    }
}
