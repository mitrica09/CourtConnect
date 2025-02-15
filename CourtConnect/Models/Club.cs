using CourtConnect.ViewModel;
using System.ComponentModel.DataAnnotations;

namespace CourtConnect.Models
{
    public class Club
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int NumberOfPlayers { get; set; }

        public virtual ICollection<User> Users { get; set; }




    }
}
