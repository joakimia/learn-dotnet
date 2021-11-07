using ELDEL_EntityFramework_Library.StaticAssets;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELDEL_EntityFramework_Library.Models
{
    [Index(nameof(UniqueId), IsUnique = true)]
    public class ChargerDetails
    {
        // Adding contructor with no input params such that the health check does not crash the program.
        public ChargerDetails()
        {
            UniqueId = ELDEL.UniqueId.GenerateShortUniqueId();
            CreatedDate = DateTime.UtcNow;
            UpdatedDate = DateTime.UtcNow;
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
        public string CreatedByEmail { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedByEmail { get; set; }

        // Navigation properties
        public long ChargerRowId { get; set; }
        public string ChargerUniqueId { get; set; }
        public virtual Charger Charger { get; set; }
    }
}
