using CourtConnect.Models;

namespace CourtConnect.ViewModel.Account
{
    public class ProfileViewModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Level { get; set; }
        public string Club { get; set; }
        public int Points { get; set; }
        public string ImageUrl { get; set; }
    }
}
