using System.ComponentModel.DataAnnotations;

namespace CourtConnect.Models
{
    public class Court
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public virtual ICollection<Announce> Announces { get; set; }

    }
}
