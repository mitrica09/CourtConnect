namespace CourtConnect.ViewModel.Match
{
    public class MatchDetailsViewModel
    {
        public string PlayerOneFullName { get; set; }
        public string PlayerTwoFullName { get; set; }
        public DateTime StartDate { get; set; }
        public string Status { get; set; }
        public string? ResultDescription { get; set; }
        public List<string> SetScores { get; set; }
    }
}
