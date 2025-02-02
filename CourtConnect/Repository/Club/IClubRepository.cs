using Microsoft.AspNetCore.Mvc.Rendering;

namespace CourtConnect.Repository.Club
{
    public interface IClubRepository
    {
        public IEnumerable<SelectListItem> GetClubForDDL();
    }
}
