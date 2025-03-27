namespace CourtConnect.ViewModel.Match
{
    public class MatchDetailsViewModel
    {
        public int Id { get; set; }

        // Jucători
        public string HostFullName { get; set; }
        public string HostLevel { get; set; }
        public int HostRank { get; set; }

        public string GuestFullName { get; set; }
        public string GuestLevel { get; set; }
        public int GuestRank { get; set; }

        // Informații despre meci
        public string LocationDetails { get; set; }
        public string StartDate { get; set; }

        public string Status { get; set; }
        public string ResultDescription { get; set; }
    }
}
