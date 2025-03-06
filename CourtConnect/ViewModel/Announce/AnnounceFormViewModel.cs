using CourtConnect.Models;
using CourtConnect.Repository.Court;
using CourtConnect.Service.Court;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CourtConnect.ViewModel.Announce
{
    public class AnnounceFormViewModel
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Titlul anuntului este obligatoriu.")]
        [StringLength(20, ErrorMessage = "Numele nu poate avea mai mult de 20 caractere")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Selectarea unui teren este obligatorie.")]
        [Range(1, int.MaxValue, ErrorMessage = "Selectează un teren valid.")]
        public int CourtId { get; set; }
        public IEnumerable<SelectListItem> Courts { get; set; }
        [DisplayName("Data si ora la care vrei sa joci")]
        public DateTime StartDate { get; set; }
        [DisplayName("Data si ora la care va expira anuntul")]
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
