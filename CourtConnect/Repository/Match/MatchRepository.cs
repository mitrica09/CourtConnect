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
            var announce = await _db.Announces.FindAsync(announceId);
            if (announce == null || !announce.ConfirmGuest || !announce.ConfirmHost)
                return null;

            var court = await _db.Courts.FindAsync(announce.CourtId);
            var location = await _db.Locations.FindAsync(court.LocationId);

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
                    SetOrder = s.Set.Id 
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
            await _rankingService.UpdatePoints(winnerUserId, winnerPoints + 50);
            await _rankingService.UpdatePoints(loserUserId, loserPoints - 50);

            // Determinăm numele câștigătorului
            var winnerUser = await _db.Users.FindAsync(winnerUserId);
            var winnerName = winnerUser?.FullName ?? "Necunoscut";

            var result = await _db.Results
                .Where(r => r.Name == winnerName)
                .FirstOrDefaultAsync();

            if (result == null)
            {
                result = new Result
                {
                    Name = winnerName
                };
                _db.Results.Add(result);
                await _db.SaveChangesAsync();
            }

            var match = await _db.Matches.FirstOrDefaultAsync(m => m.Id == matchId);
            if (match != null)
            {
                match.ResultId = result.Id;
                match.StatusId = 5; // Finalizat
                await _db.SaveChangesAsync();
            }

            return true;
        }
    }
}
