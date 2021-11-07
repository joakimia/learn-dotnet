using ELDEL_API.DTOs;
using ELDEL_API.StringExtensions;

namespace ELDEL_API.Extensions.DTOs
{
    public static class AccountCredentialExtensions
    {
        public static AccountCredentialDTO ToValidatedAccountCredentialDTO(this AccountCredentialDTO accountCredentialDTO)
        {
            if (!accountCredentialDTO.Email.IsValidEmail())
            {
                accountCredentialDTO.Message = $"Invalid or missing property 'email' (string). It must have a username@domainName.topLevelDomain structure. You provided: {accountCredentialDTO.Email}.";
                return accountCredentialDTO;
            }

            if (string.IsNullOrEmpty(accountCredentialDTO.Password))
            {
                accountCredentialDTO.Message = $"Missing property 'password' (string). You provided: {accountCredentialDTO.Email}.";
                return accountCredentialDTO;
            }

            return accountCredentialDTO;
        }
    }
}
