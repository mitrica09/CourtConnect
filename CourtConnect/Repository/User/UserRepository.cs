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
        public async Task<User> AuthenticateUserAsync(string email, string password)
        {
            var user = await _userManager.FindByNameAsync(email);
            if (user != null)
            {
                var isPassowrdValid = await _userManager.CheckPasswordAsync(user, password);
                if (isPassowrdValid)
                {
                    return user;
                }
            }
            return null;
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
            if (result.Succeeded)
            {
                var roleExists = await _roleManager.RoleExistsAsync("User");
                if (!roleExists)
                {
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole("User"));
                    if (!roleResult.Succeeded)
                    {
                        return IdentityResult.Failed(new IdentityError { Description = "Eroare la crearea rolului 'User'." });
                    }
                }
                var addRoleResult = await _userManager.AddToRoleAsync(user, "User");
                if (!addRoleResult.Succeeded)
                {
                    return IdentityResult.Failed(addRoleResult.Errors.ToArray());
                }
            }

            return result;
        }
    }
}
