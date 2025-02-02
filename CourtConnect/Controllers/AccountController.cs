using CourtConnect.Models;
using CourtConnect.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CourtConnect.Models;
using CourtConnect.ViewModel;
using static System.Runtime.InteropServices.JavaScript.JSType;
using CourtConnect.Repository.Account;
using CourtConnect.Repository.Level;
using CourtConnect.Repository.Club;

namespace CourtConnect.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly IUserRepository _userRepository;
        private readonly ILevelRepository _levelRepository;
        private readonly IClubRepository _clubRepository;


        public AccountController(SignInManager<User> signInManager
                               , UserManager<User> userManager
                               , IUserRepository userRepository
                               , IClubRepository clubRepository
                               , ILevelRepository levelRepository)
        {
            signInManager = signInManager;
            userManager = userManager;
            _userRepository = userRepository;
            _clubRepository = clubRepository;
            _levelRepository = levelRepository;
        }


        public IActionResult Register()
        {
            RegisterViewModel register = new RegisterViewModel(_clubRepository, _levelRepository);
            return View(register);
         }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            // Excludem validarea inutilă
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

            // 🔴 REPOPULARE `Clubs` și `Levels`
            register.Clubs = _clubRepository.GetClubForDDL();
            register.Levels = _levelRepository.GetLevelsForDDL();

            return View(register);
        }




        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}