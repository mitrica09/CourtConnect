using CourtConnect.ViewModel.Announce;

namespace CourtConnect.Service.Announce
{
    public interface IAnnounceService
    {
        Task<bool> Create(AnnounceFormViewModel announceForm);
    }
}
