using ELDEL_EntityFramework_Library.StaticAssets;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELDEL_EntityFramework_Library.Models
{
    [Index(nameof(UniqueId), IsUnique = true)]
    public class AccountRole : IdentityRole
    {
        public AccountRole()
        {
            UniqueId = ELDEL.UniqueId.GenerateShortUniqueId();
            CreatedDate = DateTime.UtcNow;
            UpdatedDate = DateTime.UtcNow;
        }

        // The Id is a 15 character long string (.eg: KXTR_VzGVUoOY21)
        // and is created by .NET code in memory before insertion.
        // Used by consumers and frontends, instead of exposing the row id.
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UniqueId { get; set; }
        public string Email { get; set; }
        public string RoleType { get; set; }
        public string CreatedByEmail { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedByEmail { get; set; }
    }
}
