using CourtConnect.Models;
using Microsoft.EntityFrameworkCore;
using CourtConnect.StartPackage.Database;
using CourtConnect.ViewModel.Match;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using CourtConnect.Service.Ranking;
using CourtConnect.Repository.Ranking;
using System.Security.Claims;

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
            var sets = await _db.SetsResult
                .Where(s => s.MatchId == matchId)
                .ToListAsync();

            var userWins = sets
                .GroupBy(s => s.UserId)
                .ToDictionary(g => g.Key, g => g.Count());

            var winnerUserId = userWins.OrderByDescending(g => g.Value).FirstOrDefault().Key;

            var allPlayers = await _db.UserMatches
                .Where(um => um.MatchId == matchId)
                .Select(um => um.UserId)
                .ToListAsync();

            var loserUserId = allPlayers.FirstOrDefault(u => u != winnerUserId);

            if (loserUserId == null)
            {
                throw new Exception("User not found for the loser.");
            }

            int winnerPoints = await _rankingService.GetPointsByUserId(winnerUserId);
            int loserPoints = await _rankingService.GetPointsByUserId(loserUserId);

            await _rankingService.UpdatePoints(winnerUserId, winnerPoints + 50);
            await _rankingService.UpdatePoints(loserUserId, loserPoints - 50);

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
                match.StatusId = 5;
                await _db.SaveChangesAsync();
            }

            return true;
        }

        public async Task<List<MyMatchesViewModel>> GetMyMatches()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            string userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return new List<MyMatchesViewModel>();

            var userMatches = await _db.UserMatches
                .Where(um => um.UserId == userId)
                .AsNoTracking()
                .ToListAsync();

            var matchViewModels = new List<MyMatchesViewModel>();

            foreach (var userMatch in userMatches)
            {
                var match = await _db.Matches.FindAsync(userMatch.MatchId);
                if (match == null)
                    continue;

                var announce = await _db.Announces.FindAsync(match.AnnounceId);
                if (announce == null)
                    continue;

                var status = await _db.Statuses.FindAsync(match.StatusId);
                var result = match.ResultId != 0 ? await _db.Results.FindAsync(match.ResultId) : null;

                var isHost = announce.UserId == userId;
                var opponentId = isHost ? announce.GuestUserId : announce.UserId;
                var opponent = await _userManager.FindByIdAsync(opponentId);

                // determinarea rezultatului afișat
                string rezultatFinal = "-";
                if (result != null)
                {
                    if (result.Name == opponent?.FullName)
                    {
                        rezultatFinal = "Lose";
                    }
                    else
                    {
                        rezultatFinal = "Win";
                    }
                }

                var matchVm = new MyMatchesViewModel
                {
                    MatchId = match.Id,
                    AnnounceId = announce.Id,
                    OpponentName = opponent?.FullName ?? "-",
                    MatchDate = announce.StartDate.ToString("dd/MM/yyyy HH:mm"),
                    Status = status?.Name ?? "-",
                    Result = rezultatFinal,
                    IsConfirmed = announce.ConfirmHost && announce.ConfirmGuest
                };

                matchViewModels.Add(matchVm);
            }

            return matchViewModels;
        }


    }
}
