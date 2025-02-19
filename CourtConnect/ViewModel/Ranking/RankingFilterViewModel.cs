using CourtConnect.Repository.Club;
using CourtConnect.Repository.Level;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CourtConnect.ViewModel.Ranking
{
    public class RankingFilterViewModel
    {
        [Display(Name="Nume")]
        public string Name { get; set; }

        public int ClubId { get; set; }

        public IEnumerable<SelectListItem> Clubs { get; set; }

        public int LevelId { get; set; }

        public IEnumerable<SelectListItem> Levels { get; set; }


        public RankingFilterViewModel(ILevelRepository levelRepository
                                     ,IClubRepository  clubRepository
                                     ,int clubId
                                     ,int levelId
                                     ,string name
                                     )
        {
            Levels = levelRepository.GetLevelsForDDL();
            Clubs = clubRepository.GetClubForDDL();
            this.Name = name;
            this.ClubId = clubId;
            this.LevelId = levelId;
        }


    }
}
