namespace CourtConnect.Repository.Ranking
{
    public interface IRankingRepository
    {
        Task<int> GetPointsByUserId(string userId);
    }
}
