using System.ComponentModel.DataAnnotations;

namespace CourtConnect.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int LevelId { get; set; }
        public Level Level { get; set; }

        public int ClubId { get; set; }
        public Club Club { get; set; }
        public Ranking Ranking { get; set; }


        public virtual ICollection<UserMatch> UserMatches { get; set; }
        public virtual ICollection<SetResult> SetResults { get; set; }
        public virtual ICollection<Announce> Announces { get; set; }

    }
}
