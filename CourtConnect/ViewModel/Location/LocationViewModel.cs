using System.ComponentModel;

namespace CourtConnect.ViewModel.Location
{
    public class LocationViewModel
    {
        public int Id { get; set; }
        [DisplayName("Numele orasului")]
        public string CityName { get; set; }
        [DisplayName("Numele judetului")]
        public string CountyName { get; set; }
        [DisplayName("Numele strazii")]
        public string StreetName { get; set; }
        [DisplayName("Numarul strazii")]
        public int StreetNumber { get; set; }

    }
}
