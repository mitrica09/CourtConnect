using System.Data.Entity;
using System.Security.Claims;
using CourtConnect.Models;
using CourtConnect.Repository.Ranking;
using CourtConnect.Service.Ranking;
using CourtConnect.StartPackage.Database;
using CourtConnect.ViewModel.Account;
using Microsoft.AspNetCore.Identity;

namespace CourtConnect.Repository.Account
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly CourtConnectDbContext _db;
        private readonly IRankingService _rankingService;
        

        public UserRepository(UserManager<User> userManager
                            , SignInManager<User> signInManager
                            , RoleManager<IdentityRole> roleManager
                            , CourtConnectDbContext db
                            , IRankingService rankingService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _db= db;
            _rankingService = rankingService;
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

        public async Task<ProfileViewModel> GetMyProfile(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return null; // Dacă utilizatorul nu este găsit, returnăm null
            }

            user.Club = await _db.Clubs.FindAsync(user.ClubId);
            user.Level = await _db.Levels.FindAsync(user.LevelId);

            int playerPoints = await _rankingService.GetPointsByUserId(userId);

            var currentLevel = await _db.Levels.FindAsync(user.LevelId);

            var nextLevel = _db.Levels.OrderBy(l => l.Target).FirstOrDefault(l => l.Target > playerPoints);

            int progress = 0;
            int pointsToNextLevel = 0;

            if (nextLevel != null)
            {
                progress = (int)((float)playerPoints / nextLevel.Target * 100); 
                pointsToNextLevel = nextLevel.Target - playerPoints;
            }

            var rank = _db.Rankings
                .Where(r => r.Points >= playerPoints)
                .Count();

            return new ProfileViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Level = user.Level.Name,
                Club = user.Club.Name,
                ImageUrl = user.ImageUrl,
                Points = playerPoints,
                Rank = rank,
                Progress = progress,
                NextLevel = nextLevel?.Name,
                PointsToNextLevel = pointsToNextLevel
            };
        }




        public async Task<IdentityResult> RegisterUserAsync(RegisterViewModel registerViewModel, string password)
        {

            string uniqueFileName = null;

            if (registerViewModel.Image != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(registerViewModel.Image.FileName);

                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await registerViewModel.Image.CopyToAsync(fileStream);
                }
            }

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
               ImageUrl = uniqueFileName,
               LevelId =registerViewModel.LevelId ?? 0,         
               ClubId = registerViewModel.ClubId ?? 0,
              
            };

            var selectedLevel = await _db.Levels.FindAsync(registerViewModel.LevelId);
            int points = 0; 
            if (selectedLevel != null)
            {
                points = selectedLevel.Target;
            }

            Models.Club club =await  _db.Clubs.FindAsync(registerViewModel.ClubId);
            club.NumberOfPlayers += 1;
            await _db.SaveChangesAsync();

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

            Models.Ranking rank = new Models.Ranking
            {
                UserId = user.Id,
                Points = points,
            };
            await _db.Rankings.AddAsync(rank);
            await _db.SaveChangesAsync();

            return result;
        }

      

    }
}