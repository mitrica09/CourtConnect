namespace CourtConnect.ViewModel.Match
{
    public class MatchDetailsViewModel
    {

        public int MatchId { get; set; }
        public string HostFullName { get; set; }
        public string GuestFullName { get; set; }
        public string HostLevel { get; set; }
        public string GuestLevel { get; set; }
        public int HostRank { get; set; }
        public int GuestRank { get; set; }
        public string LocationDetails { get; set; }
        public string StartDate { get; set; }
        public string Status { get; set; }
        public string ResultDescription { get; set; }
        public bool HasScores { get; set; }
        public string WinnerName { get; set; }
        public List<SetResultViewModel> SetResults { get; set; } = new();

    }
}
