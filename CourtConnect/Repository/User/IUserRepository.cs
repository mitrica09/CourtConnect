using CourtConnect.Models;
using CourtConnect.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace CourtConnect.Repository.Account
{
    public interface IUserRepository
    {
        Task<IdentityResult> RegisterUserAsync(User user, string password);
        Task<User> AuthenticateUserAsync(string email, string password);
    }
}
