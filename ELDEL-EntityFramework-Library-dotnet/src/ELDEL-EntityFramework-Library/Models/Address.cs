using ELDEL_EntityFramework_Library.StaticAssets;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELDEL_EntityFramework_Library.Models
{
    [Index(nameof(UniqueId), IsUnique = true)]
    public class Address
    {
        // Adding contructor with no input params such that the health check does not crash the program.
        public Address()
        {
        }

        // Account
        public Address(
            Account account,
            string primaryStreetName,
            string secondaryStreetName,
            string zipCode,
            string city,
            string province,
            string country,
            string countryCode,
            string createdByEmail
        )
        {
            UniqueId = ELDEL.UniqueId.GenerateShortUniqueId();
            CreatedDate = DateTime.UtcNow;
            CreatedByEmail = createdByEmail;
            UpdatedByEmail = createdByEmail;
            Account = account;
            AccountId = account.Id;
            AccountUniqueId = account.UniqueId;
            PrimaryStreetName = primaryStreetName;
            SecondaryStreetName = secondaryStreetName;
            ZipCode = zipCode;
            City = city;
            Province = province;
            Country = country;
            CountryCode = countryCode;
        }

        // Charger
        public Address(
           Charger charger,
           string primaryStreetName,
           string secondaryStreetName,
           string zipCode,
           string city,
           string province,
           string country,
           string countryCode,
           string createdByEmail
       )
        {
            UniqueId = ELDEL.UniqueId.GenerateShortUniqueId();
            CreatedDate = DateTime.UtcNow;
            CreatedByEmail = createdByEmail;
            UpdatedByEmail = createdByEmail;
            Charger = charger;
            ChargerRowId = charger.RowId;
            ChargerUniqueId = charger.UniqueId;
            PrimaryStreetName = primaryStreetName;
            SecondaryStreetName = secondaryStreetName;
            ZipCode = zipCode;
            City = city;
            Province = province;
            Country = country;
            CountryCode = countryCode;
        }

        // Row Id, obfuscated from the API consumers, primarily used for queries and optimization.
        // Use data annotation in addition to Fluent API to expclicitly
        // inform the developer that RowId is the primary key.
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RowId { get; set; }
        // The Id is a 15 character long string (.eg: KXTR_VzGVUoOY21)
        // and is created by .NET code in memory before insertion.
        // Used by consumers and frontends, instead of exposing the row id.
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UniqueId { get; set; }
        public string Description { get; set; }
        public string PrimaryStreetName { get; set; }
        public string SecondaryStreetName { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedByEmail { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedByEmail { get; set; }

        // Navigation properties
        // An address is either tied to an account or a charger
        public string AccountId { get; set; }
        public string AccountUniqueId { get; set; }
        public virtual Account Account { get; set; }
        public long? ChargerRowId { get; set; }
        public string ChargerUniqueId { get; set; }
        public Charger Charger { get; set; }
    }
}
