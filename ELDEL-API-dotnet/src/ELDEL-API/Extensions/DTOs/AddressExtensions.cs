using ELDEL_API.DTOs;
using ELDEL_API.StringExtensions;

namespace ELDEL_API.AddressExtensions
{
    public static class AddressExtensions
    {
        public static AddressDTO ToValidatedAddressDTO(this AddressDTO addressDTO)
        {
            if (!string.IsNullOrEmpty(addressDTO.Id) && !addressDTO.Id.IsValidUniqueId())
            {
                addressDTO.Message = $"Invalid property 'id' (string). You provided: {addressDTO.Id}.";
                return addressDTO;
            }

            if (!string.IsNullOrEmpty(addressDTO.PrimaryStreetName) && !addressDTO.PrimaryStreetName.IsValidStreetName())
            {
                addressDTO.Message = $"Invalid property 'primaryStreetName' (string). It must have at least 1 alphabetiacl character. You provided: {addressDTO.PrimaryStreetName}.";
                return addressDTO;
            }

            if (!string.IsNullOrEmpty(addressDTO.SecondaryStreetName) && !addressDTO.SecondaryStreetName.IsValidStreetName())
            {
                addressDTO.Message = $"Invalid property 'secondaryStreetName' (string). It must have at least 1 alphabetiacl character. You provided: {addressDTO.SecondaryStreetName}.";
                return addressDTO;
            }

            if (!string.IsNullOrEmpty(addressDTO.CountryCode) && !addressDTO.CountryCode.IsValidCountryCode())
            {
                addressDTO.Message = $"Invalid property 'countryCode' (string). It must have at exactly 2 alphabetical characters (A-Z). You provided: {addressDTO.CountryCode}.";
                return addressDTO;
            }

            return addressDTO;
        }

        public static bool IsValidStreetName(this string streetName)
        {
            try
            {
                return !streetName.IsOnlyDigits();
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidCountryCode(this string countryCode)
        {
            try
            {
                const int COUNTRY_CODE_MAX_LENGTH = 2;

                if (countryCode.Length != COUNTRY_CODE_MAX_LENGTH)
                {
                    return false;
                }

                return !countryCode.IsOnlyDigits();
            }
            catch
            {
                return false;
            }
        }
    }
}
