using CourtConnect.ViewModel.Level;
using CourtConnect.ViewModel.Location;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CourtConnect.Repository.Level
{
    public interface ILevelRepository
    {
        public IEnumerable<SelectListItem> GetLevelsForDDL();
        Task<bool> Create(LevelViewModel levelViewModel);
        Task<bool> Edit(LevelViewModel levelViewModel);
        IQueryable<LevelForDisplayViewModel> GetAllLevels();
        Task<bool> Delete(int id);
        Task<LevelViewModel> GetLevelById(int id);
    }
}
