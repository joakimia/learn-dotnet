using System;
using ELDEL_API.Models;
using ELDEL_EntityFramework_Library.Models;

namespace ELDEL_API.DTOs
{
    public class AddressDTO : ValidatedDTO
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string PrimaryStreetName { get; set; }
        public string SecondaryStreetName { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }

        internal static AddressDTO Map(Address address)
        {
            return new AddressDTO
            {
                Id = address.UniqueId,
                Description = address.Description,
                PrimaryStreetName = address.PrimaryStreetName,
                SecondaryStreetName = address.SecondaryStreetName,
                ZipCode = address.ZipCode,
                City = address.City,
                Province = address.Province,
                Country = address.Country,
                CountryCode = address.CountryCode,
            };
        }
    }
}
