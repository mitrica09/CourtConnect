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
        public int Rank { get; set; }
        public int Progress { get; set; }  // Progresul
        public string NextLevel { get; set; }  // Numele nivelului următor
        public int PointsToNextLevel { get; set; }  // Punctele necesare pentru nivelul următor

    }
}
