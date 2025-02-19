using CourtConnect.Models;
using CourtConnect.Repository.Court;
using CourtConnect.Service.Court;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CourtConnect.ViewModel.Announce
{
    public class AnnounceFormViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }    
        public int CourtId { get; set; }
        public IEnumerable<SelectListItem> Courts { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public AnnounceFormViewModel()
        {

        }

        public AnnounceFormViewModel(ICourtService courtService)
        {
            Courts = courtService.GetCourtsForDDL();
        }
    }
}
