using CourtConnect.Repository.Location;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CourtConnect.ViewModel.Court
{
    public class CourtViewModel
    {
        public int Id { get; set; }
        [DisplayName("Nume")]
        [Required(ErrorMessage = "Numele clubului este obligatoriu.")]
        [StringLength(20, ErrorMessage = "Numele nu poate avea mai mult de 20 caractere")]
        public string Name { get; set; }

        [DisplayName("Locatie")]

        [Required(ErrorMessage = "Vă rugăm să selectați o locatie.")]
        public int LocationId { get; set; } 
        public IEnumerable<SelectListItem> Locations { get; set; }

        public CourtViewModel()
        {
            
        }
        public CourtViewModel(ILocationRepository locationRepository)
        {
            Locations = locationRepository.GetLocationForDDL();
        }
    }
}
