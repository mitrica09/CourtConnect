using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CourtConnect.ViewModel.Match
{
    public class MatchResultViewModel
    {
        public int MatchId { get; set; }
        public int SetId { get; set; }
        public string UserId { get; set; }
        public string Score { get; set; }


        public IEnumerable<SelectListItem> Sets { get; set; }
        public IEnumerable<SelectListItem> Players { get; set; }
        public IEnumerable<SelectListItem> Scores { get; set; }
    }


}
