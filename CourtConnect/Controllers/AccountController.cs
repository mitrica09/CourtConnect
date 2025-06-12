using CourtConnect.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using CourtConnect.Repository.Account;
using CourtConnect.Repository.Level;
using CourtConnect.Repository.Club;
using CourtConnect.ViewModel.Account;
using System.Security.Claims;
using CourtConnect.Service.User;

namespace CourtConnect.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly IUserRepository _userRepository;
        private readonly ILevelRepository _levelRepository;
        private readonly IClubRepository _clubRepository;
        private readonly IUserService _userService;


        public AccountController(SignInManager<User> signInManager
                               , UserManager<User> userManager
                               , IUserRepository userRepository
                               , IClubRepository clubRepository
                               , ILevelRepository levelRepository
                               , IUserService userService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            _userRepository = userRepository;
            _clubRepository = clubRepository;
            _levelRepository = levelRepository;
            _userService = userService;
        }


        public IActionResult Register()
        {
            RegisterViewModel register = new RegisterViewModel(_clubRepository, _levelRepository);
            return View(register);
         }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            ModelState.Remove("Clubs");
            ModelState.Remove("Levels");

            if (ModelState.IsValid)
            {
                var result = await _userRepository.RegisterUserAsync(register, register.Password);
                if (result.Succeeded)
                {
                    TempData["Message"] = "Ai fost înregistrat cu succes.";
                    TempData["MessageType"] = "success";
                    return RedirectToAction("Index", "Home");
                }
            }

            TempData["Message"] = "Nu ai fost înregistrat";

            register.Clubs = _clubRepository.GetClubForDDL();
            register.Levels = _levelRepository.GetLevelsForDDL();

            return View(register);
        }
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var result = await signInManager.PasswordSignInAsync(
                    login.Email, login.Password, login.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                TempData["NotificationMessage"] = "Te-ai logat succes";
                TempData["NotificationType"] = "success";
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Nume sau parola gresita.");
                return View(login);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ProfileViewModel profileViewModel = await _userService.GetMyProfile(userId);
            return View(profileViewModel);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}