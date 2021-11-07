using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELDEL_EntityFramework_Library.Models
{
    public class ChargerDetailsConfiguration : IEntityTypeConfiguration<ChargerDetails>
    {
        public void Configure(EntityTypeBuilder<ChargerDetails> builder)
        {
            // Map entity to table
            builder
                .ToTable("ChargerDetails");

            // Map primary and foreign keys
            builder
                .HasKey(cd => cd.RowId);

            // Map relationships
            builder
                .HasOne(cd => cd.Charger)
                .WithOne(ch => ch.ChargerDetails)
                .HasForeignKey<ChargerDetails>(cd => cd.ChargerRowId);

            // Map properties
            builder
                .Property(cd => cd.RowId)
                .HasColumnName("RowId")
                .HasColumnType("bigint")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder
                .Property(cd => cd.UniqueId)
                .HasColumnName("UniqueId")
                .HasColumnType("char(15)")
                .IsFixedLength()
                .IsUnicode(false)
                .IsRequired();

            builder
                .Property(cd => cd.Description)
                .HasColumnName("Description")
                .HasColumnType("nvarchar(256)")
                .IsUnicode(false);

            builder
                .Property(ar => ar.CreatedByEmail)
                .HasColumnName("CreatedByEmail")
                .HasColumnType("nvarchar(128)")
                .IsUnicode(false)
                .IsRequired();

            builder
                .Property(cd => cd.CreatedDate)
                .HasColumnName("CreatedDate")
                .HasColumnType("datetime")
                .IsRequired();

            builder
                .Property(cd => cd.UpdatedDate)
                .HasColumnName("UpdatedDate")
                .HasColumnType("datetime")
                .IsRequired();

            builder
                .Property(ar => ar.UpdatedByEmail)
                .HasColumnName("UpdatedByEmail")
                .HasColumnType("nvarchar(128)")
                .IsUnicode(false)
                .IsRequired();

            builder
                .Property(cd => cd.ChargerRowId)
                .HasColumnName("ChargerRowId")
                .HasColumnType("bigint");

            builder
                .Property(cd => cd.ChargerUniqueId)
                .HasColumnName("ChargerUniqueId")
                .HasColumnType("char(15)")
                .IsUnicode(false);
        }
    }
}
