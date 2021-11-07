using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELDEL_EntityFramework_Library.Models
{
    public class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            // Map entity to table
            builder
                .ToTable("Cars");

            // Map primary and foreign keys
            builder
                .HasKey(ca => ca.RowId);

            // Map relationships
            builder
                .HasMany(ca => ca.Accounts)
                .WithMany(ac => ac.Cars);

            // Map properties
            builder
                .Property(ca => ca.RowId)
                .HasColumnName("RowId")
                .HasColumnType("bigint")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder
                .Property(ca => ca.UniqueId)
                .HasColumnName("UniqueId")
                .HasColumnType("char(15)")
                .IsFixedLength()
                .IsUnicode(false)
                .IsRequired();

            builder
                .Property(ca => ca.Name)
                .HasColumnName("Name")
                .HasColumnType("nvarchar(50)")
                .IsUnicode(false)
                .IsRequired();

            builder
                .Property(ca => ca.RegistrationPlateId)
                .HasColumnName("RegistrationPlateId")
                .HasColumnType("varchar(50)")
                .IsUnicode(false)
                .IsRequired();

            builder
                .Property(ca => ca.CreatedDate)
                .HasColumnName("CreatedDate")
                .HasColumnType("datetime")
                .IsRequired();

            builder
                .Property(ca => ca.UpdatedDate)
                .HasColumnName("UpdatedDate")
                .HasColumnType("datetime")
                .IsRequired();
        }
    }
}
