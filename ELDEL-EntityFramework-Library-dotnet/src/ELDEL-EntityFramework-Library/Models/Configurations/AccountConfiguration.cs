using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELDEL_EntityFramework_Library.Models.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            // Map entity to table
            builder
                .ToTable("Accounts");

            // ASP.NET Core Identity Maps the Guid Id column as primary key by default for Account

            // Map relationships
            builder
                 .HasOne(ac => ac.Address)
                 .WithOne(ad => ad.Account)
                 .HasForeignKey<Address>(ad => ad.AccountId);

            builder
                  .HasMany(ac => ac.Chargers)
                  .WithMany(ch => ch.Accounts);

            builder
                  .HasMany(ac => ac.Cars)
                  .WithMany(ca => ca.Accounts);

            // Map properties
            builder
                .Property(ac => ac.UniqueId)
                .HasColumnName("UniqueId")
                .HasColumnType("char(15)")
                .IsFixedLength()
                .IsUnicode(false)
                .IsRequired();

            builder
                .Property(ac => ac.FirstName)
                .HasColumnName("FirstName")
                .HasColumnType("nvarchar(75)")
                .IsUnicode(false);

            builder
                .Property(ac => ac.LastName)
                .HasColumnName("LastName")
                .HasColumnType("nvarchar(75)")
                .IsUnicode(false);

            builder
                .Property(ac => ac.FullName)
                .HasColumnName("FullName")
                .HasColumnType("nvarchar(128)")
                .IsUnicode(false);

            builder
                .Property(ac => ac.Email)
                .HasColumnName("Email")
                .HasColumnType("nvarchar(128)")
                .IsUnicode(false)
                .IsRequired();

            builder
                .Property(ac => ac.PhoneNumberPrefix)
                .HasColumnName("PhoneNumberPrefix")
                .HasColumnType("varchar(3)")
                .IsUnicode(false);

            builder
                .Property(ac => ac.CreatedDate)
                .HasColumnName("CreatedDate")
                .HasColumnType("datetime")
                .IsRequired();

            builder
                .Property(ac => ac.UpdatedDate)
                .HasColumnName("UpdatedDate")
                .HasColumnType("datetime")
                .IsRequired();

            builder
                .Property(ow => ow.AddressRowId)
                .HasColumnName("AddressRowId")
                .HasColumnType("bigint");

            builder
                .Property(ow => ow.AddressUniqueId)
                .HasColumnName("AddressUniqueId")
                .HasColumnType("char(15)")
                .IsUnicode(false);
        }
    }
}
