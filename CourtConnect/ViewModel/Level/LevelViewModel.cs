using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CourtConnect.ViewModel.Level
{
    public class LevelViewModel
    {
        public int Id { get; set; }
        [DisplayName("Nume")]
        [Required(ErrorMessage = "Numele nivelului este obligatoriu.")]

        public string Name { get; set; }
        [DisplayName("Target-ul nivelului")]
        [Required(ErrorMessage = "Target-ul nivelului este obligatoriu.")]

        public int Target {  get; set; }
    }
}
