using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CourtConnect.Models
{
    public class User :IdentityUser
    {
        public string FullName { get; set; }   
        public int LevelId { get; set; }
        public Level Level { get; set; }
        public int ClubId { get; set; }
        public Club Club { get; set; }
        public Ranking Ranking { get; set; }
        public string ImageUrl { get; set; }

        public virtual ICollection<UserMatch> UserMatches { get; set; }
        public virtual ICollection<SetResult> SetResults { get; set; }
        public virtual ICollection<Announce> Announces { get; set; }

    }
}
