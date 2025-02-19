using CourtConnect.ViewModel.Announce;

namespace CourtConnect.Repository.Announce
{
    public interface IAnnounceRepository
    {
        Task<bool> Create(AnnounceFormViewModel announceForm);
    }
}
