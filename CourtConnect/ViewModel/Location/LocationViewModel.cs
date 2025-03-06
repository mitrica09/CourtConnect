using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CourtConnect.ViewModel.Location
{
    public class LocationViewModel
    {
        public int Id { get; set; }
        [DisplayName("Numele orasului")]
        [Required(ErrorMessage = "Numele orasului este obligatoriu.")]

        public string CityName { get; set; }
        [DisplayName("Numele judetului")]
        [Required(ErrorMessage = "Numele judetului este obligatoriu.")]
        public string CountyName { get; set; }
        [DisplayName("Numele strazii")]
        [Required(ErrorMessage = "Numele strazii este obligatoriu.")]
        public string StreetName { get; set; }
        [DisplayName("Numarul strazii")]
        [Required(ErrorMessage = "Numarul strazii este obligatoriu.")]
        public int StreetNumber { get; set; }

    }
}
