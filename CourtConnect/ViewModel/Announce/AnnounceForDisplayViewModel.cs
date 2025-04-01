using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CourtConnect.ViewModel.Announce
{
    public class AnnounceForDisplayViewModel
    {
        public int Id { get; set; }        
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Courts { get; set; }
        public string Status { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

    }
}
