namespace CourtConnect.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int LevelId { get; set; }
        public Level Level { get; set; }

        public int ClubId { get; set; }
        public Club Club { get; set; }

    }
}
