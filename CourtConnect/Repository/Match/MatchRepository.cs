using CourtConnect.Models;
using Microsoft.EntityFrameworkCore;
using CourtConnect.StartPackage.Database;
using CourtConnect.ViewModel.Match;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using CourtConnect.Service.Ranking;
using CourtConnect.Repository.Ranking;

namespace CourtConnect.Repository.Match
{
    public class MatchRepository : IMatchRepository
    {
        private readonly IRankingService _rankingService;
        private readonly CourtConnectDbContext _db;

        private readonly UserManager<User> _userManager;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public MatchRepository(CourtConnectDbContext db
                                , UserManager<User> userManager
                                , IHttpContextAccessor httpContextAccessor
                                ,IRankingService rankingService)
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _rankingService = rankingService;
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


            // 4. Seturi jucate
            var setResultsRaw = await _db.SetsResult
                .Where(s => s.MatchId == matchId)
                .Include(s => s.User)
                .Include(s => s.Set)
                .ToListAsync();

            var setResults = setResultsRaw
                .Select(s => new SetResultViewModel
                {
                    PlayerName = s.User.FullName,
                    Score = s.Score,
                    SetName = s.Set.Name,
                    UserId = s.UserId,
                    SetOrder = s.Set.Id // presupunem că Set.Id reflectă ordinea (sau adaugă un câmp Order)
                })
                .ToList();


            var hasScores = await _db.SetsResult.AnyAsync(s => s.MatchId == matchId);

            return new MatchDetailsViewModel
            {
                MatchId = match.Id,
                HostFullName = hostUser.FullName,
                GuestFullName = guestUser.FullName,

                HostLevel = hostUser.Level.Name,
                GuestLevel = guestUser.Level.Name,

                HostRank = hostRank,
                GuestRank = guestRank,

                LocationDetails = $"{location.County} - {location.City}, {location.Street}, {location.Number}",
                StartDate = announce.StartDate.ToString("dd MMM yyyy, HH:mm"),

                Status = match.Status?.Name,
                ResultDescription = match.Result?.Name,

                HasScores = hasScores,
                SetResults = setResults
            };
        }

        public IEnumerable<SelectListItem> GetPlayersForDDL(int matchId)
        {
            var players = _db.UserMatches
                             .Where(um => um.MatchId == matchId)
                             .Select(um => new SelectListItem
                             {
                                 Value = um.UserId,
                                 Text = um.User.FullName
                             })
                             .ToList();

            return players;
        }

        public IEnumerable<SelectListItem> GetScoresForDDL()
        {
            List<string> scores = new List<string>
            {
                "6-0", "6-1", "6-2", "6-3", "6-4", "7-5", "7-6"
            };

            return scores.Select(score => new SelectListItem
            {
                Value = score,
                Text = score
            }).ToList();
        }

        public IEnumerable<SelectListItem> GetSetsForDDL()
        {
            return _db.Sets
                      .Select(set => new SelectListItem
                      {
                          Value = set.Id.ToString(),
                          Text = set.Name
                      })
                      .ToList();
        }

        public MatchResultViewModel GetScoreForDDL(int matchId)
        {
            return new MatchResultViewModel
            {
                MatchId = matchId,
                Sets = GetSetsForDDL(),
                Players = GetPlayersForDDL(matchId),
                Scores = GetScoresForDDL()
            };
        }

        public async Task<bool> CreateResultMatch(MatchResultViewModel model)
        {
            if (model.SetResults == null || !model.SetResults.Any())
                return false;

            foreach (var set in model.SetResults)
            {
                var setResult = new SetResult
                {
                    MatchId = model.MatchId,
                    SetId = set.SetId,
                    UserId = set.UserId,
                    Score = set.Score
                };

                _db.SetsResult.Add(setResult);
            }

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetScoreAlreadyExists(int matchId, int setId)
        {
            return await _db.SetsResult
                .AnyAsync(s => s.MatchId == matchId && s.SetId == setId);
        }

        public async Task<Models.Match> GetMatchById(int matchId)
        {
            return await _db.Matches.FindAsync(matchId);
        }


        public async Task<bool> DeclareWinner(int matchId)
        {
            // Obținem seturile asociate cu acest meci
            var sets = await _db.SetsResult
                .Where(s => s.MatchId == matchId)
                .ToListAsync();

            // Grupăm seturile pe utilizator și numărăm câte seturi a câștigat fiecare
            var userWins = sets
                .GroupBy(s => s.UserId)
                .ToDictionary(g => g.Key, g => g.Count());

            // Căutăm câștigătorul pe baza numărului de seturi câștigate
            var winnerUserId = userWins.OrderByDescending(g => g.Value).FirstOrDefault().Key;

            // Obținem utilizatorii care au participat la meci folosind tabela UserMatches
            var allPlayers = await _db.UserMatches
                .Where(um => um.MatchId == matchId)
                .Select(um => um.UserId)
                .ToListAsync();

            // Verificăm cine este pierzătorul
            var loserUserId = allPlayers.FirstOrDefault(u => u != winnerUserId);

            // Verificăm dacă loserUserId este valid
            if (loserUserId == null)
            {
                throw new Exception("User not found for the loser.");
            }

            // Obținem punctele actuale pentru câștigător și pierzător
            int winnerPoints = await _rankingService.GetPointsByUserId(winnerUserId);
            int loserPoints = await _rankingService.GetPointsByUserId(loserUserId);

            // Actualizăm punctele
            await _rankingService.UpdatePoints(winnerUserId, winnerPoints + 50);  // Câștigătorul primește 50 de puncte
            await _rankingService.UpdatePoints(loserUserId, loserPoints - 50);    // Pierzătorul pierde 50 de puncte

            // Determinăm numele câștigătorului
            var winnerUser = await _db.Users.FindAsync(winnerUserId);
            var winnerName = winnerUser?.FullName ?? "Necunoscut";

            // Verificăm dacă există deja un rezultat cu acest nume
            var result = await _db.Results
                .Where(r => r.Name == winnerName)
                .FirstOrDefaultAsync();

            if (result == null)
            {
                // Dacă nu există, adăugăm un nou rezultat
                result = new Result
                {
                    Name = winnerName
                };
                _db.Results.Add(result);
                await _db.SaveChangesAsync();
            }

            // Actualizăm meciul cu rezultatul și statusul
            var match = await _db.Matches.FirstOrDefaultAsync(m => m.Id == matchId);
            if (match != null)
            {
                match.ResultId = result.Id;
                match.StatusId = 5; // Finalizat
                await _db.SaveChangesAsync();
            }

            return true;
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
        // Se va adauga in tabela din baza de date SetResult (SetId,MatchId,UserId)
        // In SetsResult vei mai adauga un camp de tip string Score in care vei introduce scorul  
        // Pentru dropDown ca sa iti vina doar jucatorii din acel meci ii vei lua dupa Id-urile din anunt
        // In matchRepository voi creea functia de: CreateResultMatch
        // Creez un VM numit MatchResultViewModel
        //Se va schimba statusul meciul in terminat dupa ce salvez tot in db.



        //Creez o metoda numita DeclaraCastigator(), ii dau ca parametrii announceId si MatchId, de aici iau ambii useri si vad in lista din SetsResult care user castiga(dupa matchId) 
        // cred ca: am o functie care deja iti cauta in lista dupa o valoare si vede de cate ori apare si dupa daca e cazul fac comparatie sa vad care e castigatorul
        // trebuie sa te uiti si in anuntul meciului ca sa iei ambii jucatori
        // iei lista SetsResult dupa match Id si iei doar USer 
        //Vezi care user apare de mai multe ori iar acela a castigat meciul 
        //Cine a castigat meciul cauti unde are el punctele(faci cautare dupa userId) si la acel user adaugi 3 puncte si la cine a pierdut ii scazi 3 puncte 

    }
}
