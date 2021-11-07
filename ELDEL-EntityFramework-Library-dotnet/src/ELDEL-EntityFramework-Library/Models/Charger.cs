using ELDEL_EntityFramework_Library.StaticAssets;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELDEL_EntityFramework_Library.Models
{
    [Index(nameof(UniqueId), IsUnique = true)]
    public class Charger
    {
        // Adding contructor with no input params such that the health check does not crash the program.
        public Charger()
        {
        }

        public Charger(string name, Account account, string createdByEmail)
        {
            UniqueId = ELDEL.UniqueId.GenerateShortUniqueId();
            Name = name ?? "New charger";
            Accounts = new List<Account>() { account };
            CreatedDate = DateTime.UtcNow;
            UpdatedDate = DateTime.UtcNow;
            CreatedByEmail = createdByEmail;
            UpdatedByEmail = createdByEmail;
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
        public string Name { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string SocketType { get; set; }
        public string ManufacturerType { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedByEmail { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedByEmail { get; set; }

        // Navigation properties
        public long? AddressRowId { get; set; }
        public string AddressUniqueId { get; set; }
        public virtual Address Address { get; set; }
        public long? ChargerDetailsRowId { get; set; }
        public string ChargerDetailsUniqueId { get; set; }
        public virtual ChargerDetails ChargerDetails { get; set; }
        public virtual IList<Account> Accounts { get; set; }
    }
}
