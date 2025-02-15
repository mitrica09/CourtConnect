using CourtConnect.ViewModel.Account;
using System.Security.Claims;

namespace CourtConnect.Service.User
{
    public interface IUserService
    {
        Task<ProfileViewModel> GetMyProfile(string userId);
    }
}
