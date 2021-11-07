using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELDEL_EntityFramework_Library.Models
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            // Map entity to table
            builder
                .ToTable("Addresses");

            // Map primary key
            builder
                .HasKey(ad => ad.RowId);

            // Map relationships
            builder
                .HasOne(ad => ad.Charger)
                .WithOne(ch => ch.Address)
                .HasForeignKey<Charger>(ch => ch.AddressRowId);

            builder
                .HasOne(ad => ad.Charger)
                .WithOne(ch => ch.Address)
                .HasForeignKey<Charger>(ch => ch.AddressRowId);

            builder
                .HasOne(ad => ad.Account)
                .WithOne(ac => ac.Address)
                .HasForeignKey<Account>(ac => ac.AddressRowId);

            // Map properties
            builder
                .Property(ad => ad.RowId)
                .HasColumnName("RowId")
                .HasColumnType("bigint")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder
                .Property(ad => ad.UniqueId)
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
                .Property(ad => ad.PrimaryStreetName)
                .HasColumnName("PrimaryStreetName")
                .HasColumnType("nvarchar(75)")
                .IsUnicode(false);

            builder
                .Property(ad => ad.SecondaryStreetName)
                .HasColumnName("SecondaryStreetName")
                .HasColumnType("nvarchar(75)")
                .IsUnicode(false);

            builder
                .Property(ad => ad.ZipCode)
                .HasColumnName("ZipCode")
                .HasColumnType("varchar(5)")
                .IsUnicode(false);

            builder
                .Property(ad => ad.City)
                .HasColumnName("City")
                .HasColumnType("nvarchar(50)")
                .IsUnicode(false);

            builder
                .Property(ad => ad.Province)
                .HasColumnName("Province")
                .HasColumnType("nvarchar(50)")
                .IsUnicode(false);

            builder
                .Property(ad => ad.Country)
                .HasColumnName("Country")
                .HasColumnType("nvarchar(50)")
                .IsUnicode(false);

            builder
                .Property(ad => ad.CountryCode)
                .HasColumnName("CountryCode")
                .HasColumnType("varchar(10)")
                .IsUnicode(false);

            builder
                .Property(ar => ar.CreatedByEmail)
                .HasColumnName("CreatedByEmail")
                .HasColumnType("nvarchar(128)")
                .IsUnicode(false)
                .IsRequired();

            builder
                .Property(ad => ad.CreatedDate)
                .HasColumnName("CreatedDate")
                .HasColumnType("datetime")
                .IsRequired();

            builder
                .Property(ad => ad.UpdatedDate)
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
                .Property(ad => ad.AccountId)
                .HasColumnName("AccountId")
                .IsUnicode(false);

            builder
                .Property(ad => ad.AccountUniqueId)
                .HasColumnName("AccountUniqueId")
                .HasColumnType("char(15)")
                .IsUnicode(false);

            builder
                .Property(ad => ad.ChargerRowId)
                .HasColumnName("ChargerRowId")
                .HasColumnType("bigint");

            builder
                .Property(ad => ad.ChargerUniqueId)
                .HasColumnName("ChargerUniqueId")
                .HasColumnType("char(15)")
                .IsUnicode(false);
        }
    }
}
