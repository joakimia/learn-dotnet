using ELDEL_API.Models;

namespace ELDEL_API.DTOs
{
    public class EldelJWTAccessTokenDTO : ValidatedDTO
    {
        public string AccessToken { get; set; }
    }
}
