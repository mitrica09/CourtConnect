using System.ComponentModel.DataAnnotations;

namespace CourtConnect.Models
{
    public class AnnounceGuestUser
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int AnnounceId { get; set; }
        public Announce Announce { get; set; }
    }
}
