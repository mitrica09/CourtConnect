using CourtConnect.ViewModel.Location;

namespace CourtConnect.Service.Location
{
    public interface ILocationService
    {
        Task<bool> Create(LocationViewModel locationViewModel);
    }
}
