using CourtConnect.Repository.Level;
using CourtConnect.ViewModel.Level;

namespace CourtConnect.Service.Level
{
    public class LevelService : ILevelService
    {
        private readonly ILevelRepository _levelRepository;

        public LevelService(ILevelRepository levelRepository)
        {
            _levelRepository = levelRepository;
        }
        public async Task<bool> Create(LevelViewModel levelViewModel)
        {
            return await _levelRepository.Create(levelViewModel);
        }
    }
}
