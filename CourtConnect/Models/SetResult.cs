using System.ComponentModel.DataAnnotations;

namespace CourtConnect.Models
{
    public class SetResult
    {
        [Key]
        public int Id { get; set; }
        public int SetId { get; set; }
        public string Score { get; set; }
        public Set Set {  get; set; }
        public int MatchId { get; set; }
        public Match Match { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

    }
}
