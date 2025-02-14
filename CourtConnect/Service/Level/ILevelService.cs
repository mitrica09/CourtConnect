using CourtConnect.ViewModel.Level;

namespace CourtConnect.Service.Level
{
    public interface ILevelService
    {
        Task<bool> Create(LevelViewModel levelViewModel);
        Task<bool> Edit(LevelViewModel levelViewModel);
        IQueryable<LevelForDisplayViewModel> GetAllLevels();
        Task<bool> Delete(int id);
        Task<LevelViewModel> GetLevelById(int id);
    }
}
