namespace ELDEL_API.StringExtensions
{
    public static class StringExtensions
    {
        public static bool IsValidEmail(this string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidPhoneNumberPrefix(this string phoneNumberPrefix)
        {
            try
            {
                var isDigits = phoneNumberPrefix.IsOnlyDigits();
                var hasValidLength = phoneNumberPrefix.Length <= 3;

                return isDigits && hasValidLength;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidPhoneNumber(this string phoneNumber)
        {
            try
            {
                var isDigits = phoneNumber.IsOnlyDigits();
                var maxPhoneNumberLength = 15;
                var hasValidLength = phoneNumber.Length < maxPhoneNumberLength;

                return isDigits && hasValidLength;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidUniqueId(this string uniqueId)
        {
            try
            {
                const int UNIQUE_ID_FIXED_LENGTH = 15;

                if (uniqueId.Length != UNIQUE_ID_FIXED_LENGTH)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        // TODO: double check this one
        public static bool ContainsDigits(this string inputString)
        {
            try
            {
                if (string.IsNullOrEmpty(inputString))
                {
                    return false;
                }

                // Use old-school loop for efficiency
                for (int index = 0; index < inputString.Length; index++)
                {
                    // Use one-time comparison weird check for efficiency 
                    if ((inputString[index] ^ '0') < 9)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsOnlyDigits(this string inputString)
        {
            try
            {
                if (string.IsNullOrEmpty(inputString))
                {
                    return false;
                }

                // Use old-school loop for efficiency
                for (int index = 0; index < inputString.Length; index++)
                {
                    // Use one-time comparison weird check for efficiency 
                    if ((inputString[index] ^ '0') > 9)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
