using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CourtConnect.ViewModel.Club
{
    public class ClubViewModel
    {
        public int Id { get; set; }
        [DisplayName("Nume")]
        [Required(ErrorMessage = "Numele clubului este obligatoriu.")]
        public string Name { get; set; }
        [DisplayName("Numarul de playeri din club")]
        public int NumberOfPlayers { get; set; }
    }
}
