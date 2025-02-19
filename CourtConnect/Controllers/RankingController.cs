using CourtConnect.Repository.Club;
using CourtConnect.Repository.Level;
using CourtConnect.Repository.Ranking;
using CourtConnect.Service.Ranking;
using CourtConnect.ViewModel.Ranking;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace CourtConnect.Controllers
{
    public class RankingController : Controller
    {

        int _pageSize = 5;
        public RankingViewModel RankingVm { get; set; }
        private readonly IRankingService _rankingService;
        private readonly ILevelRepository _levelRepository;
        private readonly IClubRepository  _clubRepository;

        public RankingController(IRankingService rankingService
                                , ILevelRepository levelRepository
                                , IClubRepository  clubRepository)
        {
            RankingVm = new RankingViewModel();

            _rankingService = rankingService;
            _levelRepository = levelRepository;
            _clubRepository = clubRepository;
        }

        public  void SetFilterData(int levelId
                                 ,int clubId
                                 ,string name
                                 ,int? page)
        {
            List<RankingForDisplayViewModel> ranking = _rankingService.GetRanking().Result.ToList();
            int pageNumber = page ?? 1;

            if (levelId !=0)
            {
                ranking =  _rankingService.GetRankingByLevelId(ranking,levelId).Result.ToList();
            }

            if (clubId != 0)
            {
                ranking =  _rankingService.GetRankingByClubId(ranking, clubId).Result.ToList();
            }

            if (name != null)
            {
                ranking =  _rankingService.GetRankingByName(ranking, name).Result.ToList();
            }

            RankingVm.Ranking = new StaticPagedList<RankingForDisplayViewModel>(
            ranking.Skip((pageNumber - 1) * _pageSize).Take(_pageSize),
            pageNumber,
            _pageSize,
            ranking.Count
        ); ;
            RankingVm.Filter = new RankingFilterViewModel(_levelRepository, _clubRepository, clubId, levelId, name);
        }

        [HttpGet]
        public async Task<IActionResult> Index(int clubId,int levelId,string name,int? page)
        {
            SetFilterData(levelId,clubId,name,page);
            return View(RankingVm);
        }
    }
}