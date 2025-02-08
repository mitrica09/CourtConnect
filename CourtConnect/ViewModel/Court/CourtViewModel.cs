using CourtConnect.Repository.Location;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;

namespace CourtConnect.ViewModel.Court
{
    public class CourtViewModel
    {
        public string Name { get; set; }

        [Required(ErrorMessage = "Vă rugăm să selectați o locatie.")]
        public int? LocationId { get; set; }
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
