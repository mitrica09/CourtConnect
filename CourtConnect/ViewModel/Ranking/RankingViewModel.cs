using CourtConnect.Repository.Club;
using CourtConnect.Repository.Level;
using X.PagedList;

namespace CourtConnect.ViewModel.Ranking
{
    public class RankingViewModel
    {
        public StaticPagedList<RankingForDisplayViewModel> Ranking { get; set; }

        public RankingFilterViewModel Filter { get; set; }

        public RankingViewModel(ILevelRepository levelRepository
                                     , IClubRepository clubRepository
                                     , int clubId
                                     , int levelId
                                     , string name
                                     )
        {
            this.Filter = new RankingFilterViewModel(levelRepository
                                                    ,clubRepository
                                                    ,clubId
                                                    ,levelId
                                                    ,name);
        }

        public RankingViewModel()
        {
            
        }
    }
}
