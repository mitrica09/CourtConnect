namespace CourtConnect.Service.Ranking
{
    public interface IRankingService
    {        
        public Task<int> GetPointsByUserId(string userId);
    }
}
