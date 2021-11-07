using ELDEL_EntityFramework_Library.StaticAssets;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELDEL_EntityFramework_Library.Models
{
    [Index(nameof(UniqueId), IsUnique = true)]
    public class Account : IdentityUser
    {
        public Account(string email)
        {
            UniqueId = ELDEL.UniqueId.GenerateShortUniqueId();
            CreatedDate = DateTime.UtcNow;
            UpdatedDate = DateTime.UtcNow;
            UpdatedByEmail = email;
            UserName = email;
            Email = email;
            EmailConfirmed = false;
        }

        // The Id is a 15 character long string (.eg: KXTR_VzGVUoOY21)
        // and is created by .NET code in memory before insertion.
        // Used by consumers and frontends, instead of exposing the row id.
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UniqueId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string PhoneNumberPrefix { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedByEmail { get; set; }

        // Navigation properties
        public virtual Address Address { get; set; }
        public long? AddressRowId { get; set; }
        public string AddressUniqueId { get; set; }
        public virtual IList<Charger> Chargers { get; set; }
        public virtual IList<Car> Cars { get; set; }
    }
}
