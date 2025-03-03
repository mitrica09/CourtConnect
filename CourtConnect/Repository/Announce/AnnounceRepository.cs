using CourtConnect.Models;
using CourtConnect.StartPackage.Database;
using CourtConnect.ViewModel.Announce;
using CourtConnect.ViewModel.Court;
using Microsoft.AspNetCore.Identity;
using System.Data.Entity;
using System.Security.Claims;

namespace CourtConnect.Repository.Announce
{
    public class AnnounceRepository : IAnnounceRepository
    {
        private readonly CourtConnectDbContext _db;

        private readonly UserManager<User> _userManager;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public AnnounceRepository(CourtConnectDbContext db
                                , UserManager<User> userManager
                                , IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Create(AnnounceFormViewModel announceForm)
        {         

            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var user = _httpContextAccessor.HttpContext?.User;
                    string userID = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    Models.Announce announce = new Models.Announce
                    {

                        Name = announceForm.Name,
                        UserId = userID,
                        CourtId = announceForm.CourtId,
                        StartDate = announceForm.StartDate,
                        EndDate = announceForm.EndDate,
                        AnnounceStatusId = 1,
                    };
                    _db.Announces.Add(announce);
                    await _db.SaveChangesAsync();
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }

        }

        public  async Task<List<AnnounceForDisplayViewModel>> GetAllAnnounces()
        {
            var announce =  _db.Announces.ToList();
            List<AnnounceForDisplayViewModel> announceForDisplayViewModels = new List<AnnounceForDisplayViewModel>();

            foreach(var item in announce)
            {
                AnnounceForDisplayViewModel announceForDisplayViewModel = new AnnounceForDisplayViewModel();
                announceForDisplayViewModel.StartDate = item.StartDate.ToString();
                announceForDisplayViewModel.EndDate = item.EndDate.ToString();
                announceForDisplayViewModel.Courts = _db.Courts.Where(s=>s.Id == item.CourtId).Select(s=>s.Name).FirstOrDefault();
                announceForDisplayViewModel.Status = _db.AnnouncesStatus.Where(s=>s.Id == item.AnnounceStatusId).Select(s=>s.Name).FirstOrDefault();
                announceForDisplayViewModel.ImageUrl = _db.Users.Where(s => s.Id == item.UserId).Select(s => s.ImageUrl).FirstOrDefault();
                announceForDisplayViewModel.Name = item.Name;
                announceForDisplayViewModel.Id = item.Id;
                announceForDisplayViewModels.Add(announceForDisplayViewModel);                 

            }
            return announceForDisplayViewModels;



        }

        public async Task<AnnounceDetailsViewModel> GetAnnounceDetails(int announceId, string userId)
        {
            var guestUser = await _userManager.FindByIdAsync(userId);

            guestUser.Club = await _db.Clubs.FindAsync(guestUser.ClubId);
            guestUser.Level = await _db.Levels.FindAsync(guestUser.LevelId);
            var guestRanking = _db.Rankings.Where(s => s.UserId == guestUser.Id).FirstOrDefault();

            var announce = await _db.Announces.FindAsync(announceId);
            announce.Court = await _db.Courts.FindAsync(announce.CourtId);
        

            var hostUser = await _userManager.FindByIdAsync(announce.UserId);

            hostUser.Club = await _db.Clubs.FindAsync(hostUser.ClubId);
            hostUser.Level = await _db.Levels.FindAsync(hostUser.LevelId);
            var hostRanking = _db.Rankings.Where(s => s.UserId == hostUser.Id).FirstOrDefault();

            var location = await _db.Locations.FindAsync(announce.Court.LocationId);

            return new AnnounceDetailsViewModel
            { 
              HostFullName = hostUser.FullName,
              GuestFullName = guestUser.FullName,
              GuestLevel = guestUser.Level.Name,
              HostLevel = hostUser.Level.Name,
              GuestRank = guestRanking.Points,
              HostRank = hostRanking.Points,
              LocationDetails = location.County + "-" + location.City + "," + location.Street + "," + location.Number,
              StartDate = announce.StartDate.ToString(),
              GuestClub = guestUser.Club.Name,
              HostClub = hostUser.Club.Name      
            };



        }
    }
}

/*
 * 
 * !!!!Sa fii logat cand esti guest user si vrei sa dai play pe un anunt.
 De facut:
- sa poti sa pui doar un singur anunt per utilizator
- sa se schimbe statusul anuntului in functie de anunt, adica daca e acceptat de cineva sa treaca la statusul respectiv
- sa poti sa nu mai scoti anuntul daca este deja acceptat de cineva
- sa primesti notificare daca cineva ti-a acceptat anuntul
 */ 