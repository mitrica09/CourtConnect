using System.ComponentModel.DataAnnotations;

namespace CourtConnect.Models
{
    public class Level
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Target { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
