using CourtConnect.Models;
using CourtConnect.StartPackage.Database;
using CourtConnect.ViewModel.Announce;
using CourtConnect.ViewModel.Court;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CourtConnect.Repository.Announce
{
    public class AnnounceRepository : IAnnounceRepository
    {
        private readonly CourtConnectDbContext _db;
       
        private readonly IHttpContextAccessor _httpContextAccessor;

        

        public AnnounceRepository(CourtConnectDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
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
                        AnnounceStatusId = 4,
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
    }
}

/*
 De facut:
- sa poti sa pui doar un singur anunt per utilizator
- sa se schimbe statusul anuntului in functie de anunt, adica daca e acceptat de cineva sa treaca la statusul respectiv
- sa poti sa nu mai scoti anuntul daca este deja acceptat de cineva
- sa primesti notificare daca cineva ti-a acceptat anuntul
 */ 