using System.ComponentModel.DataAnnotations;

namespace CourtConnect.Models
{
    public class Match
    {
        [Key]
        public int Id { get; set; }
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public int ResultId { get; set; }
        public Result Result { get; set; }
        public int AnnounceId { get; set; }
        public Announce Announce { get; set; }

        public virtual ICollection<UserMatch> UserMatches { get; set; }
        public virtual ICollection<SetResult> SetResults { get; set; }
    }
}
