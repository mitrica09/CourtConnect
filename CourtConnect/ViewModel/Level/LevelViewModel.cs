using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CourtConnect.ViewModel.Level
{
    public class LevelViewModel
    {
        public int Id { get; set; }
        [DisplayName("Nume")]
        public string Name { get; set; }
        [DisplayName("Target-ul nivelului")]
        public int Target {  get; set; }
    }
}
