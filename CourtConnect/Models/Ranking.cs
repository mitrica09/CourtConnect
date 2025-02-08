using System.ComponentModel.DataAnnotations;

namespace CourtConnect.Models
{
    public class Ranking
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        public  User User { get; set; }
        public int Points {  get; set; }
        

    }
}
