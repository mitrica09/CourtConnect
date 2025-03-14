using System.Security.Claims;
using CourtConnect.Models;
using CourtConnect.ViewModel.Account;
using Microsoft.AspNetCore.Identity;

namespace CourtConnect.Repository.Account
{
    public interface IUserRepository
    {
        Task<IdentityResult> RegisterUserAsync(RegisterViewModel registerViewModel, string password);
        Task<User> AuthenticateUserAsync(string email, string password);
        Task<ProfileViewModel> GetMyProfile(string userId);
    }
}
/*
 *In profilul meu, daca am un anunt postat, sa mi apara detalii despre anunt
 *Sa pot face update la detaliile contului (sa nu pot modifica nivelul si punctele), doar clubul din care fac parte, numele, emailul. 
 *Daca alegi sa modifici clubul din my profile, automat sa scazi numarul de playeri din acel club cu 1 si sa incrementezi la numarul nou creat 
cu 1 pentru a avea o evidenta clara de cati jucatori sunt intr un club
 *
 *
 */