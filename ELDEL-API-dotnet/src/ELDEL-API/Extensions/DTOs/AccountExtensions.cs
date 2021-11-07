using ELDEL_API.DTOs;
using ELDEL_API.StringExtensions;

namespace ELDEL_API.AccountCredentialExtensions
{
    public static class AccountExtensions
    {
        public static AccountDTO ToValidatedAccountDTO(this AccountDTO accountDTO)
        {
            if (!accountDTO.Email.IsValidEmail())
            {
                accountDTO.Message = $"Invalid or missing property 'email' (string). It must have a username@domainName.topLevelDomain structure. You provided: {accountDTO.Email}.";
                return accountDTO;
            }

            if (!string.IsNullOrEmpty(accountDTO.PhoneNumberPrefix) && !accountDTO.PhoneNumberPrefix.IsValidPhoneNumberPrefix())
            {
                accountDTO.Message = $"Invalid or missing property 'phoneNumberPrefix' (string). It must have at least/most 1-3 characters, containing only numbers from 0-9. You provided: {accountDTO.PhoneNumberPrefix}.";
                return accountDTO;
            }

            if (!string.IsNullOrEmpty(accountDTO.PhoneNumber) && !accountDTO.PhoneNumber.IsValidPhoneNumber())
            {
                accountDTO.Message = $"Invalid or missing property 'phoneNumber' (string). It must have at least/most 5-13 characters, containing only numbers. You provided: {accountDTO.PhoneNumber}.";
                return accountDTO;
            }

            return accountDTO;
        }
    }
}
