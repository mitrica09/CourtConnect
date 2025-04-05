namespace CourtConnect.ViewModel.Match
{
    public class MyMatchesViewModel
    {
        public int MatchId { get; set; }
        public string OpponentName { get; set; }
        public string MatchDate { get; set; }
        public string Status { get; set; }
        public string Result { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
