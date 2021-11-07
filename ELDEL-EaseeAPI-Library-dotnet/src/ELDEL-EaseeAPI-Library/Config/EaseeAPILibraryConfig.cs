namespace ELDEL_EaseeAPI_Library.Config
{
    public interface IEaseeAPILibraryConfig
    {
        string BaseUri { get; }
        string Username { get; }
        string Password { get; }
        string GrantType { get; }
        string LoginEndpoint { get; }
        string RefreshTokenEndpoint { get; }
        string ChargerEndpoint { get; }
    }

    public class EaseeAPILibraryConfig : IEaseeAPILibraryConfig
    {
        public string BaseUri { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string GrantType { get; set; }

        public string LoginEndpoint
        {
            get => $"api/accounts/token";
        }


        public string RefreshTokenEndpoint
        {
            get => $"api/accounts/refresh_token";
        }

        public string ChargerEndpoint
        {
            get => $"api/chargers";
        }
    }
}
