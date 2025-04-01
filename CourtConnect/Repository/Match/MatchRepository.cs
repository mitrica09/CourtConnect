using CourtConnect.Models;
using CourtConnect.StartPackage.Database;
using CourtConnect.ViewModel.Match;
using Microsoft.AspNetCore.Identity;

namespace CourtConnect.Repository.Match
{
    public class MatchRepository : IMatchRepository
    {
        private readonly CourtConnectDbContext _db;

        private readonly UserManager<User> _userManager;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public MatchRepository(CourtConnectDbContext db
                                , UserManager<User> userManager
                                , IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<MatchDetailsViewModel> GetMatchDetails(int announceId, int matchId)
        {
            // 1. Ia anunțul și detalii locație
            var announce = await _db.Announces.FindAsync(announceId);
            if (announce == null || !announce.ConfirmGuest || !announce.ConfirmHost)
                return null;

            var court = await _db.Courts.FindAsync(announce.CourtId);
            var location = await _db.Locations.FindAsync(court.LocationId);

            // 2. Ia userii
            var hostUser = await _userManager.FindByIdAsync(announce.UserId);
            var guestUser = await _userManager.FindByIdAsync(announce.GuestUserId);

            hostUser.Level = await _db.Levels.FindAsync(hostUser.LevelId);
            guestUser.Level = await _db.Levels.FindAsync(guestUser.LevelId);

            var hostRank = _db.Rankings.FirstOrDefault(r => r.UserId == hostUser.Id)?.Points ?? 0;
            var guestRank = _db.Rankings.FirstOrDefault(r => r.UserId == guestUser.Id)?.Points ?? 0;

            // 3. Ia match-ul real (status, rezultat)
            var match = await _db.Matches.FindAsync(matchId);
            match.Status = await _db.Statuses.FindAsync(match.StatusId);
            match.Result = match.ResultId != null
                ? await _db.Results.FindAsync(match.ResultId)
                : null;

            return new MatchDetailsViewModel
            {
                HostFullName = hostUser.FullName,
                GuestFullName = guestUser.FullName,

                HostLevel = hostUser.Level.Name,
                GuestLevel = guestUser.Level.Name,

                HostRank = hostRank,
                GuestRank = guestRank,

                LocationDetails = $"{location.County} - {location.City}, {location.Street}, {location.Number}",
                StartDate = announce.StartDate.ToString("dd MMM yyyy, HH:mm"),

                Status = match.Status?.Name,
                ResultDescription = match.Result?.Name
            };
        }





        //Functie CreateMatch() - ori o faci in match repository ori in announceRepository 
        // creeazi un match in tabela match pui Id-ul la statusul in curs 
        // result pui initial null 
        //iei anuntul tau duoa Id
        // intri in tabela usermatch si vei creea doua inregistrari una cu hostId si guestUserId si respectv meciId la fiecare


        //Creezi pagina de meci
        //Faci un ViewModel cu pagina meciului, date despre jucatori, date despre meci ,cote pariuri,si mai vezi tu 
        //In pagina de meci vei avea un buton pe care sa apesti startMeci si va porni un cronometru, butonul se va face stop meci iar status va vi pe asteptare scor 





        // In pagina de meci vei avea un buton de finalizeaza meci(se va trimitre Id-ul meciului)
        // dupa accesare va aparea o pagina in care vei introduce scorul meciului (Dropdown cu setul,dropdown cu jucatorul si scorul setului) iti va veni in backend o lista cu acestea)
        // Se va adauga in tabela din baza de date SetResul (SetId,MatchId,UserId)
        // In SetsResult vei mai adauga un camp de tip string Score in care vei introduce scorul  
        // Pentru dropDown ca sa iti vina doar jucatorii din acel meci ii vei lua dupa Id-urile din anunt
        // In matchRepository voi creea functia de: CreateResultMatch
        // Creez un VM numit MatchResultViewModel
        //Se va schimba statusul meciul in terminat dupa ce salvez tot in db.
    }
}
