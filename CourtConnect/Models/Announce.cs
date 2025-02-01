using System.ComponentModel.DataAnnotations;

namespace CourtConnect.Models
{
    public class Announce
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int CourtId { get; set; }
        public Court Court { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int AnnounceStatusId { get; set; }
        public AnnounceStatus AnnounceStatus { get; set; }

    }
}
