using CourtConnect.Repository.Court;
using CourtConnect.ViewModel.Club;
using CourtConnect.ViewModel.Court;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CourtConnect.Service.Court
{
    public class CourtService : ICourtService
    {
        private readonly ICourtRepository _courtRepository;

        public CourtService(ICourtRepository courtRepository)
        {
            _courtRepository = courtRepository;
        }

        public async Task<bool> Create(CourtViewModel courtViewModel)
        {
            return await _courtRepository.Create(courtViewModel);
        }

        public Task<bool> Delete(int id)
        {
            return _courtRepository.Delete(id);
        }

        public async Task<bool> Edit(CourtViewModel courtViewModel)
        {
            return await _courtRepository.Edit(courtViewModel);
        }

        public IQueryable<CourtForDisplayViewModel> GetAllCourts()
        {
            return _courtRepository.GetAllCourts();
        }

        public Task<CourtViewModel> GetCourtById(int id)
        {
            return _courtRepository.GetCourtById(id);
        }

        public IEnumerable<SelectListItem> GetCourtsForDDL()
        {
            return _courtRepository.GetCourtsForDDL();
        }
    }


}
