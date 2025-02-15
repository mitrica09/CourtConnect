using Microsoft.AspNetCore.Mvc.Rendering;

namespace CourtConnect.Repository.Level
{
    public interface ILevelRepository
    {
        public IEnumerable<SelectListItem> GetLevelsForDDL();
    }
}
