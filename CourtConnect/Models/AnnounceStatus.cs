using System.ComponentModel.DataAnnotations;

namespace CourtConnect.Models
{
    public class AnnounceStatus
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Announce> Announces { get; set; }

    }
}
