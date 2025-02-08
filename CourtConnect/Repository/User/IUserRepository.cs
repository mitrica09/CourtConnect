using CourtConnect.Models;
using CourtConnect.ViewModel.Account;
using Microsoft.AspNetCore.Identity;

namespace CourtConnect.Repository.Account
{
    public interface IUserRepository
    {
        Task<IdentityResult> RegisterUserAsync(RegisterViewModel registerViewModel, string password);
        Task<User> AuthenticateUserAsync(string email, string password);
    }
}
