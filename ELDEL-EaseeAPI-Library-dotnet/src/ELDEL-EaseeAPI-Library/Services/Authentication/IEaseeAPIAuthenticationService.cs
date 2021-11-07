using System.Threading.Tasks;

namespace ELDEL_EaseeAPI_Library.Services
{
    public interface IEaseeAPIAuthenticationService
    {
        string AccessToken { get; }
        string RefreshToken { get; }
        Task AuthorizeAsync();
        Task ReAuthorizeAsync();
    }
}
