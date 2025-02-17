using CourtConnect.Models;
using CourtConnect.StartPackage.Database;
using CourtConnect.ViewModel.Level;
using CourtConnect.ViewModel.Location;
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

        public async Task<bool> Edit(LevelViewModel levelViewModel)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    Models.Level level = await _db.Levels.FindAsync(levelViewModel.Id);
                    if (level == null)
                        return false;
                    level.Name = levelViewModel.Name;
                    level.Target = levelViewModel.Target;
                    await _db.SaveChangesAsync();
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public IQueryable<LevelForDisplayViewModel> GetAllLevels()
        {
            return _db.Levels.Select(c => new LevelForDisplayViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Target = c.Target
            });
        }


        public async Task<bool> Delete(int id)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    Models.Level level = await _db.Levels.FindAsync(id);
                    if (level == null)
                        return false;

                    _db.Levels.Remove(level);
                    await _db.SaveChangesAsync();
                    transaction.Commit();
                    return true;

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public async Task<LevelViewModel> GetLevelById(int id)
        {
            var level = await _db.Levels.FindAsync(id);
            if (level == null) return null;

            return new LevelViewModel
            {
                Id = level.Id,
                Name = level.Name,
                Target = level.Target,
            };
        }
    }
}
