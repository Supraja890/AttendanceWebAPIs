namespace WHOTrackingWebAPI.Models
{
    public class UserPresence
    {
        public string UserName { get; set; }
        public string IpAddress { get; set; }
        private bool _isOffice;
        public bool IsOffice {
            get => IpAddress != null && IpAddress.StartsWith("10.10.10.");
            set => _isOffice = value;
        }
    }
    public class UserPresenceDto
    {
        public string UserName { get; set; }
        public string IpAddress { get; set; }
    }
}
