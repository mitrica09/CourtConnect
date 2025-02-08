using CourtConnect.Models;
using CourtConnect.ViewModel.Court;
using System.Collections.Generic;

namespace CourtConnect.Repository.Court
{
    public interface ICourtRepository
    {
        Task<bool> Create(CourtViewModel courtViewModel);

    }
}
