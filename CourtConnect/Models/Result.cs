using System.ComponentModel.DataAnnotations;

namespace CourtConnect.Models
{
    public class Result
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Match> Matches { get; set; }
    }
}
