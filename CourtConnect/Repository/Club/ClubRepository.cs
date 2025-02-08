using CourtConnect.StartPackage.Database;
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
    }
}
