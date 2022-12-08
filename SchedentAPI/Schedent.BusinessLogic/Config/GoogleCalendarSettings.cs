namespace Schedent.BusinessLogic.Config
{
    // Model used for mapping the GoogleCalendarSettings from the appsettings
    public class GoogleCalendarSettings
    {
        public string ClientId { get; set; }
        public string ProjectId { get; set; }
        public string AuthUri { get; set; }
        public string TokenUri { get; set; }
        public string AuthProviderX509CertUrl { get; set; }
        public string ClientSecret { get; set; }
        public string[] RedirectUris { get; set; }
    }
}
