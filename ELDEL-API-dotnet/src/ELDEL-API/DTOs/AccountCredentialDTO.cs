using ELDEL_API.Models;

namespace ELDEL_API.DTOs
{
    public class AccountCredentialDTO : ValidatedDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
