using System.ComponentModel.DataAnnotations;

namespace CourtConnect.Models
{
    public class Location
    {
        [Key]
        public int Id { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Street { get; set; }
        public int Number {  get; set; }

        public virtual ICollection<Court> Courts { get; set; }

    }
}
