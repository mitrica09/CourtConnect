using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CourtConnect.ViewModel.Match
{
    public class MatchResultItemViewModel
    {
        public int SetId { get; set; }
        public string UserId { get; set; }
        public string Score { get; set; }
    }

    public class MatchResultViewModel
    {
        public int MatchId { get; set; }

        public List<MatchResultItemViewModel> SetResults { get; set; } = new(); // ADĂUGAT

        [BindNever]
        public IEnumerable<SelectListItem> Sets { get; set; }

        [BindNever]
        public IEnumerable<SelectListItem> Players { get; set; }

        [BindNever]
        public IEnumerable<SelectListItem> Scores { get; set; }
    }
}
