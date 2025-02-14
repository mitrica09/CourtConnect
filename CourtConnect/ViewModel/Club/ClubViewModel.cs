using System.ComponentModel;

namespace CourtConnect.ViewModel.Club
{
    public class ClubViewModel
    {
        public int Id { get; set; }
        [DisplayName("Nume")]
        public string Name { get; set; }
        [DisplayName("Numarul de playeri din club")]
        public int NumberOfPlayers { get; set; }
    }
}
