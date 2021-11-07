using ELDEL_API.DTOs;

namespace ELDEL_API.Services
{
    public interface IAuthenticationService
    {
        EldelJWTAccessTokenDTO AuthorizeByAccountDTO(AccountDTO account);
        AccountDTO ValidateToken(string token);
    }
}
