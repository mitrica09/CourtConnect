using CourtConnect.ViewModel.Announce;

namespace CourtConnect.Service.Announce
{
    public interface IAnnounceService
    {
        Task<bool> Create(AnnounceFormViewModel announceForm);
        Task<List<AnnounceForDisplayViewModel>> GetAllAnnounces();
        Task<AnnounceDetailsViewModel> GetAnnounceDetails(int announceId, string userId);

    }
}
