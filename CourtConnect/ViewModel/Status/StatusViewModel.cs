using System.ComponentModel;

namespace CourtConnect.ViewModel.Status
{
    public class StatusViewModel
    {
        public int Id { get; set; }
        [DisplayName("Nume")]
        public string Name { get; set; }
    }
}
