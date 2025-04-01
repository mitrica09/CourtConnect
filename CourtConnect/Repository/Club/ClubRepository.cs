using CourtConnect.StartPackage.Database;
using CourtConnect.ViewModel.Club;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.Entity;

namespace CourtConnect.Repository.Club
{
    public class ClubRepository : IClubRepository
    {
        private readonly CourtConnectDbContext _db;

        public ClubRepository(CourtConnectDbContext db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetClubForDDL()
        {
            List<SelectListItem> Clubs = new List<SelectListItem>();
            IEnumerable<Models.Club> ListClubs = _db.Clubs;
            foreach (var club in ListClubs)
            {
                Clubs.Add(new SelectListItem
                {
                    Value = club.Id.ToString(),
                    Text = club.Name,
                });

            }
            return Clubs;
        }

        public async Task<bool> Create(ClubViewModel clubViewModel)
        {
            using(var transation = _db.Database.BeginTransaction())
            {
                try
                {
                    string name = _db.Clubs.Where(c => c.Name == clubViewModel.Name).Select(s=>s.Name).FirstOrDefault();
                    if (name == null)
                    {

                        Models.Club club = new Models.Club
                        {
                            Name = clubViewModel.Name,
                            NumberOfPlayers = 0,
                        };
                        _db.Clubs.Add(club);
                        await _db.SaveChangesAsync();
                        transation.Commit();
                        return true;
                    }else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    transation.Rollback();
                    throw ex;
                }
            }
        }

        public async Task<bool> Edit(ClubViewModel clubViewModel)
        {
            using (var transaction =_db.Database.BeginTransaction())
            {
                try
                {
                    Models.Club club = await _db.Clubs.FindAsync(clubViewModel.Id);
                    if (club == null)
                        return false;

                    club.Name = clubViewModel.Name;
                    //club.NumberOfPlayers = clubViewModel.NumberOfPlayers; - doar daca vreau sa resetez numarul de jucatori la 0 atunci cand editez numele.

                    await _db.SaveChangesAsync();
                    transaction.Commit();
                    return true;
                }catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public IQueryable<ClubForDisplayViewModel> GetAllClubs()
        {
            return _db.Clubs.Select(c => new ClubForDisplayViewModel
            {
                Name = c.Name,
                NumberOfPlayers = c.NumberOfPlayers,
                Id = c.Id
            });
        }

        public async Task<bool> Delete(int id)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    Models.Club club = await _db.Clubs.FindAsync(id);
                    if(club == null) 
                      return false;

                    _db.Clubs.Remove(club);
                    await _db.SaveChangesAsync();
                    transaction.Commit();
                    return true;

                }catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public async Task<ClubViewModel> GetClubById(int id)
        {
            var club = await _db.Clubs.FindAsync(id);
            if (club == null) return null;

            return new ClubViewModel
            {
                Id = club.Id,
                Name = club.Name,
                NumberOfPlayers = club.NumberOfPlayers,
            };
        }
    }
}
