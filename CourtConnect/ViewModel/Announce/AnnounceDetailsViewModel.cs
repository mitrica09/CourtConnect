using System.Globalization;

namespace CourtConnect.ViewModel.Announce
{
    public class AnnounceDetailsViewModel 
    {  
        public int Id { get; set; }
        public string HostFullName { get; set; }
        public string GuestFullName { get; set; }
        public string GuestLevel { get; set; }
        public string HostLevel { get; set; }
        public int GuestRank { get; set; }
        public int HostRank { get; set; }
        public string LocationDetails { get; set; }
        public string StartDate { get; set;}        
        public string GuestClub { get; set;}
        public string HostClub { get; set;}



        public bool ConfirmGuest { get; set; }
        public bool ConfirmHost { get; set; }
        public bool IsHost { get; set; }
    }
}
