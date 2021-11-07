using System;
using System.Text.Json.Serialization;
using ELDEL_API.Models;
using ELDEL_EntityFramework_Library.Models;

namespace ELDEL_API.DTOs
{
    public class AccountDTO : ValidatedDTO
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumberPrefix { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public AddressDTO Address { get; set; }

        internal static AccountDTO Map(Account account)
        {
            return new AccountDTO
            {
                Id = account.UniqueId,
                FirstName = account.FirstName,
                LastName = account.LastName,
                FullName = account.FullName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
                PhoneNumberPrefix = account.PhoneNumberPrefix,
                Address = account.Address == null ? null : AddressDTO.Map(account.Address)
            };
        }
    }
}
