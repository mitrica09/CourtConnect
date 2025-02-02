using System.ComponentModel.DataAnnotations;

namespace CourtConnect.Models
{
    public class UserMatch
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int MatchId { get; set; }
        public Match Match { get; set; }

    }
}
