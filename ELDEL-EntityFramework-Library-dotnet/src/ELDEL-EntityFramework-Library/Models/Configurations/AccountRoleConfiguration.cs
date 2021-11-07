using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ELDEL_EntityFramework_Library.Models.Configurations
{
    public class AccountRoleConfiguration : IEntityTypeConfiguration<AccountRole>
    {
        public void Configure(EntityTypeBuilder<AccountRole> builder)
        {
            // Map entity to table
            builder
                .ToTable("AccountRoles");

            // ASP.NET Core Identity Maps the Guid Id column as primary key by default for AccountRole

            // Map relationships

            // Map properties
            builder
                .Property(ar => ar.UniqueId)
                .HasColumnName("UniqueId")
                .HasColumnType("char(15)")
                .IsFixedLength()
                .IsUnicode(false)
                .IsRequired();

            builder
                .Property(ar => ar.RoleType)
                .HasColumnName("RoleType")
                .HasColumnType("varchar(50)")
                .IsUnicode(false)
                .IsRequired();

            builder
                .Property(ar => ar.CreatedByEmail)
                .HasColumnName("CreatedByEmail")
                .HasColumnType("nvarchar(128)")
                .IsUnicode(false)
                .IsRequired();

            builder
                .Property(ar => ar.CreatedDate)
                .HasColumnName("CreatedDate")
                .HasColumnType("datetime")
                .IsRequired();

            builder
                .Property(ar => ar.UpdatedDate)
                .HasColumnName("UpdatedDate")
                .HasColumnType("datetime")
                .IsRequired();

            builder
                .Property(ar => ar.UpdatedByEmail)
                .HasColumnName("UpdatedByEmail")
                .HasColumnType("nvarchar(128)")
                .IsUnicode(false)
                .IsRequired();
        }
    }
}
