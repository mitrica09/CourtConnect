using System.ComponentModel.DataAnnotations;

namespace CourtConnect.Models
{
    public class Set
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<SetResult> SetResults { get; set; }

    }
}
