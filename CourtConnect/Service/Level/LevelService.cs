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

        public Task<bool> Delete(int id)
        {
            return _levelRepository.Delete(id);
        }

        public async Task<bool> Edit(LevelViewModel levelViewModel)
        {
            return await _levelRepository.Edit(levelViewModel);
        }

        public IQueryable<LevelForDisplayViewModel> GetAllLevels()
        {
            return _levelRepository.GetAllLevels();
        }

        public Task<LevelViewModel> GetLevelById(int id)
        {
            return _levelRepository.GetLevelById(id);
        }
    }
}
