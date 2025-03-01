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
    }
}

/*
 De facut:
- sa poti sa pui doar un singur anunt per utilizator
- sa se schimbe statusul anuntului in functie de anunt, adica daca e acceptat de cineva sa treaca la statusul respectiv
- sa poti sa nu mai scoti anuntul daca este deja acceptat de cineva
- sa primesti notificare daca cineva ti-a acceptat anuntul
 */ 