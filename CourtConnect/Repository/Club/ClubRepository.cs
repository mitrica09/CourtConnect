using CourtConnect.StartPackage.Database;
using CourtConnect.ViewModel.Club;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public async Task<bool> Create(ClubModelView clubModelView)
        {
            using(var transation = _db.Database.BeginTransaction())
            {
                try
                {
                    Models.Club club = new Models.Club
                    {
                        Name = clubModelView.Name,
                        NumberOfPlayers = 0,
                    };
                    _db.Clubs.Add(club);
                    await _db.SaveChangesAsync();
                    transation.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transation.Rollback();
                    throw ex;
                }
            }
        }
    }
}
