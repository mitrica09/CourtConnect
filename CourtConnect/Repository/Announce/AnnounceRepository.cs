using CourtConnect.Models;
using CourtConnect.StartPackage.Database;
using CourtConnect.ViewModel.Announce;
using CourtConnect.ViewModel.Court;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
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

        public async Task<bool> ConfirmGuest(int announceId, string userId)
        {
            var announce = await _db.Announces.FindAsync(announceId);
            if (announce == null) 
                return false;
           
            if (announce.ConfirmGuest) 
                return false;

            announce.ConfirmGuest = true;
            announce.GuestUserId = userId;
            _db.Announces.Update(announce);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ConfirmHost(int announceId, string userId)
        {
            var announce = await _db.Announces.FindAsync(announceId);

            if (announce == null || !announce.ConfirmGuest)
                return false;

            if (announce.UserId != userId)
                return false;

            announce.ConfirmHost = true;

            // 🔹 Verificăm dacă și Guest a confirmat, atunci schimbăm statusul anunțului
            if (announce.ConfirmGuest && announce.ConfirmHost)
            {
                announce.AnnounceStatusId = 2; // Setăm statusul "Meci acceptat"
            }

            _db.Announces.Update(announce);
            await _db.SaveChangesAsync();

            return true;
        }


        public async Task<bool> RejectGuest(int announceId, string userId)
        {
            var announce = await _db.Announces.FindAsync(announceId);

            if (announce == null || announce.UserId != userId)
                return false;

            announce.GuestUserId = null;
            announce.ConfirmGuest = false;

            _db.Announces.Update(announce);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Create(AnnounceFormViewModel announceForm)
        {         

            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var user = _httpContextAccessor.HttpContext?.User;
                    string userID = user.FindFirst(ClaimTypes.NameIdentifier).Value;

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

        public async Task<List<AnnounceForDisplayViewModel>> GetAllAnnounces()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            string userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Adăugăm înapoi anunțurile care fie nu au fost confirmate deloc, fie Guest-ul a renunțat
            var announces = _db.Announces
                .Where(a => a.GuestUserId == null || (a.ConfirmGuest == false && a.ConfirmHost == false)) // Afișăm doar anunțurile disponibile
                .AsNoTracking()
                .ToList();

            if (!string.IsNullOrEmpty(userId))
            {
                // Dacă utilizatorul este logat, excludem anunțurile postate de el
                announces = announces.Where(a => a.UserId != userId).ToList();
            }

            List<AnnounceForDisplayViewModel> announceForDisplayViewModels = new List<AnnounceForDisplayViewModel>();

            foreach (var item in announces)
            {
                AnnounceForDisplayViewModel announceForDisplayViewModel = new AnnounceForDisplayViewModel
                {
                    StartDate = item.StartDate.ToString(),
                    EndDate = item.EndDate.ToString(),
                    Courts = _db.Courts.Where(s => s.Id == item.CourtId).Select(s => s.Name).FirstOrDefault(),
                    Status = _db.AnnouncesStatus.Where(s => s.Id == item.AnnounceStatusId).Select(s => s.Name).FirstOrDefault(),
                    ImageUrl = _db.Users.Where(s => s.Id == item.UserId).Select(s => s.ImageUrl).FirstOrDefault(),
                    Name = item.Name,
                    Id = item.Id
                };

                announceForDisplayViewModels.Add(announceForDisplayViewModel);
            }

            return announceForDisplayViewModels;
        }




        public async Task<AnnounceDetailsViewModel> GetAnnounceDetails(int announceId, string userId)
        {
            var announce = await _db.Announces.FindAsync(announceId);
            if (announce == null) return null;

            announce.Court = await _db.Courts.FindAsync(announce.CourtId);
            var location = await _db.Locations.FindAsync(announce.Court.LocationId);

            var hostUser = await _userManager.FindByIdAsync(announce.UserId);
            hostUser.Club = await _db.Clubs.FindAsync(hostUser.ClubId);
            hostUser.Level = await _db.Levels.FindAsync(hostUser.LevelId);
            var hostRanking = _db.Rankings.Where(s => s.UserId == hostUser.Id).FirstOrDefault();

            var guestUser = await _userManager.FindByIdAsync(userId);
            guestUser.Club = await _db.Clubs.FindAsync(guestUser.ClubId);
            guestUser.Level = await _db.Levels.FindAsync(guestUser.LevelId);
            var guestRanking = _db.Rankings.Where(s => s.UserId == guestUser.Id).FirstOrDefault();

            return new AnnounceDetailsViewModel
            {
                Id = announce.Id,
                HostFullName = hostUser.FullName,
                GuestFullName = guestUser.FullName,
                GuestLevel = guestUser.Level.Name,
                HostLevel = hostUser.Level.Name,
                GuestRank = guestRanking.Points,
                HostRank = hostRanking.Points,
                LocationDetails = location.County + "-" + location.City + "," + location.Street + "," + location.Number,
                StartDate = announce.StartDate.ToString(),
                GuestClub = guestUser.Club.Name,
                HostClub = hostUser.Club.Name,

                ConfirmGuest = announce.ConfirmGuest,
                ConfirmHost = announce.ConfirmHost,

                IsHost = userId == announce.UserId
            };
        }

        public async Task<List<AnnounceDetailsViewModel>> GetMyAnnounces()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            string userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return new List<AnnounceDetailsViewModel>(); 
            }

            var hostAnnounces = _db.Announces
                .Where(a => a.UserId == userId) 
                .AsNoTracking()
                .ToList();

            // Selectam anunturile unde utilizatorul este GUEST
            var guestAnnounces = _db.Announces
                .Where(a => a.GuestUserId == userId) //Selectează DOAR anunturile unde utilizatorul s-a inscris
                .AsNoTracking()
                .ToList();

            List<AnnounceDetailsViewModel> announceDetailsViewModels = new List<AnnounceDetailsViewModel>();

            foreach (var item in hostAnnounces)
            {
                var guestUser = await _userManager.FindByIdAsync(item.GuestUserId);
                if (guestUser != null)
                {
                    guestUser.Club = await _db.Clubs.FindAsync(guestUser.ClubId);
                    guestUser.Level = await _db.Levels.FindAsync(guestUser.LevelId);
                    var guestRanking = _db.Rankings.FirstOrDefault(s => s.UserId == guestUser.Id);

                    var court = await _db.Courts.FindAsync(item.CourtId);
                    var location = await _db.Locations.FindAsync(court.LocationId);

                    announceDetailsViewModels.Add(new AnnounceDetailsViewModel
                    {
                        Id = item.Id,
                        GuestFullName = guestUser.FullName,
                        GuestLevel = guestUser.Level.Name,
                        GuestRank = guestRanking?.Points ?? 0,
                        LocationDetails = location.County + "-" + location.City + "," + location.Street + "," + location.Number,
                        StartDate = item.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                        GuestClub = guestUser.Club.Name,
                        ConfirmGuest = item.ConfirmGuest,
                        ConfirmHost = item.ConfirmHost,
                        IsHost = true
                    });
                }
            }

            foreach (var item in guestAnnounces)
            {
                var hostUser = await _userManager.FindByIdAsync(item.UserId);
                hostUser.Club = await _db.Clubs.FindAsync(hostUser.ClubId);
                hostUser.Level = await _db.Levels.FindAsync(hostUser.LevelId);
                var hostRanking = _db.Rankings.FirstOrDefault(s => s.UserId == hostUser.Id);

                var court = await _db.Courts.FindAsync(item.CourtId);
                var location = await _db.Locations.FindAsync(court.LocationId);

                announceDetailsViewModels.Add(new AnnounceDetailsViewModel
                {
                    Id = item.Id,
                    GuestFullName = hostUser.FullName, // Afisam numele Host-ului, nu al Guest-ului
                    GuestLevel = hostUser.Level.Name,
                    GuestRank = hostRanking?.Points ?? 0,
                    LocationDetails = location.County + "-" + location.City + "," + location.Street + "," + location.Number,
                    StartDate = item.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    GuestClub = hostUser.Club.Name,
                    ConfirmGuest = item.ConfirmGuest,
                    ConfirmHost = item.ConfirmHost,
                    IsHost = false
                });
            }

            return announceDetailsViewModels;
        }

        public async Task<int?> CreateMatch(int announceId)
        {
            var announce = await _db.Announces.FindAsync(announceId);
            if (announce == null || !announce.ConfirmGuest || !announce.ConfirmHost)
                return null;

            using (var transaction = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    var match = new Models.Match
                    {
                        StatusId = 1, // presupunem că 1 = Match Confirmed
                        ResultId = 1,
                        AnnounceId = announceId,
                        UserMatches = new List<UserMatch>
                {
                    new UserMatch { UserId = announce.UserId },
                    new UserMatch { UserId = announce.GuestUserId }
                }
                    };

                    _db.Matches.Add(match);

                    // Schimbăm și statusul anunțului dacă e nevoie (ex: 2 = „match creat”)
                    announce.AnnounceStatusId = 2;
                    _db.Announces.Update(announce);

                    await _db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return match.Id;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}


