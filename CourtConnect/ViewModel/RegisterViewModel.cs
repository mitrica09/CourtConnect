using CourtConnect.Repository.Club;
using CourtConnect.Repository.Level;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace CourtConnect.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "The {0} must be at {2} and at max {1} characters long.")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword", ErrorMessage = "Password does not match.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Vă rugăm să selectați un nivel.")]
        public int? LevelId { get; set; }
        public IEnumerable<SelectListItem> Levels { get; set; }

        [Required(ErrorMessage = "Vă rugăm să selectați un club.")]
        public int? ClubId { get; set; }
       
        public IEnumerable<SelectListItem> Clubs { get; set; }


        [Required(ErrorMessage = "Confirm Password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        public RegisterViewModel()
        {
            
        }
       public RegisterViewModel(IClubRepository clubRepository,ILevelRepository  levelRepository)
        {
            Clubs = clubRepository.GetClubForDDL();
            Levels = levelRepository.GetLevelsForDDL();
        }
    }
}
