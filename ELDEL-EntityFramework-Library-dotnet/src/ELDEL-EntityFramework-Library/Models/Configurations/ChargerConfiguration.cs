using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELDEL_EntityFramework_Library.Models
{
    public class ChargerConfiguration : IEntityTypeConfiguration<Charger>
    {
        public void Configure(EntityTypeBuilder<Charger> builder)
        {
            // Map entity to table
            builder
                .ToTable("Chargers");

            // Map primary and foreign keys
            builder
                .HasKey(ch => ch.RowId);

            // Map relationships
            builder
                .HasOne(ch => ch.ChargerDetails)
                .WithOne(cd => cd.Charger)
                .HasForeignKey<Charger>(ch => ch.ChargerDetailsRowId);

            builder
                .HasOne(ch => ch.Address);

            builder
                .HasMany(ch => ch.Accounts)
                .WithMany(ac => ac.Chargers);

            // Map properties
            builder
                .Property(ch => ch.RowId)
                .HasColumnName("RowId")
                .HasColumnType("bigint")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder
                .Property(ch => ch.UniqueId)
                .HasColumnName("UniqueId")
                .HasColumnType("char(15)")
                .IsFixedLength()
                .IsUnicode(false)
                .IsRequired();

            builder
                .Property(ch => ch.Name)
                .HasColumnName("Name")
                .HasColumnType("nvarchar(75)")
                .IsUnicode(false)
                .IsRequired();

            builder
                .Property(ch => ch.Longitude)
                .HasColumnName("Longitude")
                .HasColumnType("decimal(9,6)");

            builder
                .Property(ch => ch.Latitude)
                .HasColumnName("Latitude")
                .HasColumnType("decimal(9,6)");

            builder
                .Property(ch => ch.SocketType)
                .HasColumnName("SocketType")
                .HasColumnType("nvarchar(25)")
                .IsUnicode(false);

            builder
                .Property(ch => ch.ManufacturerType)
                .HasColumnName("ManufacturerType")
                .HasColumnType("nvarchar(50)")
                .IsUnicode(false);

            builder
                .Property(ch => ch.CreatedDate)
                .HasColumnName("CreatedDate")
                .HasColumnType("datetime")
                .IsRequired();

            builder
                .Property(ar => ar.CreatedByEmail)
                .HasColumnName("CreatedByEmail")
                .HasColumnType("nvarchar(128)")
                .IsUnicode(false)
                .IsRequired();

            builder
                .Property(ch => ch.UpdatedDate)
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
                .Property(ch => ch.AddressRowId)
                .HasColumnName("AddressRowId")
                .HasColumnType("bigint");

            builder
                .Property(ch => ch.AddressUniqueId)
                .HasColumnName("AddressUniqueId")
                .HasColumnType("char(15)")
                .IsUnicode(false);

            builder
                .Property(ch => ch.ChargerDetailsRowId)
                .HasColumnName("ChargerDetailsRowId")
                .HasColumnType("bigint");

            builder
                .Property(ch => ch.ChargerDetailsUniqueId)
                .HasColumnName("ChargerDetailsUniqueId")
                .HasColumnType("char(15)")
                .IsUnicode(false);
        }
    }
}
