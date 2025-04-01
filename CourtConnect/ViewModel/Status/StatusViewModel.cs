using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CourtConnect.ViewModel.Status
{
    public class StatusViewModel
    {
        public int Id { get; set; }
        [DisplayName("Nume")]
        [Required(ErrorMessage = "Numele statusului este obligatoriu.")]
        public string Name { get; set; }
    }
}
