using System.Security.Claims;
using CourtConnect.Repository.Account;
using CourtConnect.ViewModel.Account;

namespace CourtConnect.Service.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<ProfileViewModel> GetMyProfile(string userId)
        {
            return _userRepository.GetMyProfile(userId);
        }
    }
}
