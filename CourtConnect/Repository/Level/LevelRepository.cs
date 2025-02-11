using CourtConnect.Models;
using CourtConnect.StartPackage.Database;
using CourtConnect.ViewModel.Level;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CourtConnect.Repository.Level
{
    public class LevelRepository : ILevelRepository
    {
        private readonly CourtConnectDbContext _db;

        public LevelRepository(CourtConnectDbContext db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetLevelsForDDL()
        {
            List<SelectListItem> Levels = new List<SelectListItem>();
            IEnumerable<Models.Level> LevelsCategories = _db.Levels;
            foreach(var level in LevelsCategories)
            {
                Levels.Add(new SelectListItem
                { 
                  Value = level.Id.ToString(),
                  Text = level.Name,

                });

            }
            return Levels;
        }

        public async Task<bool> Create(LevelViewModel levelViewModel)
        {
            using(var transition = _db.Database.BeginTransaction())
            {
                try
                {
                    Models.Level level = new Models.Level
                    {
                        Name = levelViewModel.Name,
                        Target = levelViewModel.Target
                    };
                    _db.Levels.Add(level);
                    await _db.SaveChangesAsync();
                    transition.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transition.Rollback();
                    throw ex;
                }
            }
        }

    }
}
