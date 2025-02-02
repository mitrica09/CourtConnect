using CourtConnect.Models;
using CourtConnect.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace CourtConnect.Repository.Account
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRepository(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public Task<User> AuthenticateUserAsync(string email, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterViewModel registerViewModel, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerViewModel.Email);
            if(existingUser!=null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Adresa de email este deja folosită." });
            }
            var user = new User
            {
               UserName =registerViewModel.Email,
               FullName=registerViewModel.FullName,
               Email =registerViewModel.Email,
               LevelId =registerViewModel.LevelId ?? 0,         
               ClubId = registerViewModel.ClubId ?? 0,
              
            };

            var result = await _userManager.CreateAsync(user, registerViewModel.Password);
            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
            }

            return result;
        }
    }
}
