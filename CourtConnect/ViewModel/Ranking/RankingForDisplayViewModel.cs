namespace CourtConnect.ViewModel.Ranking
{
    public class RankingForDisplayViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }

        public int Points { get; set; }

        public int ClubId { get; set; }
        public string Club { get; set; }

        public int LevelId { get; set; }
        public string Level { get; set; }
    }
}
