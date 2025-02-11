using CourtConnect.ViewModel.Level;

namespace CourtConnect.Service.Level
{
    public interface ILevelService
    {
        Task<bool> Create(LevelViewModel levelViewModel);
    }
}
