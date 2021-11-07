﻿// <auto-generated />
using System;
using ELDEL_EntityFramework_Library;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ELDEL_EntityFramework_Library.Migrations
{
    [DbContext(typeof(EldelContext))]
    partial class EldelContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.8")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AccountCar", b =>
                {
                    b.Property<string>("AccountsId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("CarsRowId")
                        .HasColumnType("bigint");

                    b.HasKey("AccountsId", "CarsRowId");

                    b.HasIndex("CarsRowId");

                    b.ToTable("AccountCar");
                });

            modelBuilder.Entity("AccountCharger", b =>
                {
                    b.Property<string>("AccountsId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("ChargersRowId")
                        .HasColumnType("bigint");

                    b.HasKey("AccountsId", "ChargersRowId");

                    b.HasIndex("ChargersRowId");

                    b.ToTable("AccountCharger");
                });

            modelBuilder.Entity("ELDEL_EntityFramework_Library.Models.Account", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<long?>("AddressRowId")
                        .HasColumnType("bigint")
                        .HasColumnName("AddressRowId");

                    b.Property<string>("AddressUniqueId")
                        .HasColumnType("char(15)")
                        .HasColumnName("AddressUniqueId");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime")
                        .HasColumnName("CreatedDate");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)")
                        .HasColumnName("Email");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsUnicode(false)
                        .HasColumnType("nvarchar(75)")
                        .HasColumnName("FirstName");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(128)")
                        .HasColumnName("FullName");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(75)")
                        .HasColumnName("LastName");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("PhoneNumberPrefix")
                        .HasColumnType("varchar(3)")
                        .HasColumnName("PhoneNumberPrefix");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UniqueId")
                        .IsRequired()
                        .IsUnicode(false)
                        .HasColumnType("char(15)")
                        .HasColumnName("UniqueId")
                        .IsFixedLength(true);

                    b.Property<string>("UpdatedByEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime")
                        .HasColumnName("UpdatedDate");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("UniqueId")
                        .IsUnique();

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("ELDEL_EntityFramework_Library.Models.Address", b =>
                {
                    b.Property<long>("RowId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("RowId")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccountId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("AccountId");

                    b.Property<string>("AccountUniqueId")
                        .HasColumnType("char(15)")
                        .HasColumnName("AccountUniqueId");

                    b.Property<long?>("ChargerRowId")
                        .HasColumnType("bigint")
                        .HasColumnName("ChargerRowId");

                    b.Property<string>("ChargerUniqueId")
                        .HasColumnType("char(15)")
                        .HasColumnName("ChargerUniqueId");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("City");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("Country");

                    b.Property<string>("CountryCode")
                        .HasColumnType("varchar(10)")
                        .HasColumnName("CountryCode");

                    b.Property<string>("CreatedByEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasColumnName("CreatedByEmail");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime")
                        .HasColumnName("CreatedDate");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(256)")
                        .HasColumnName("Description");

                    b.Property<string>("PrimaryStreetName")
                        .HasColumnType("nvarchar(75)")
                        .HasColumnName("PrimaryStreetName");

                    b.Property<string>("Province")
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("Province");

                    b.Property<string>("SecondaryStreetName")
                        .HasColumnType("nvarchar(75)")
                        .HasColumnName("SecondaryStreetName");

                    b.Property<string>("UniqueId")
                        .IsRequired()
                        .IsUnicode(false)
                        .HasColumnType("char(15)")
                        .HasColumnName("UniqueId")
                        .IsFixedLength(true);

                    b.Property<string>("UpdatedByEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasColumnName("UpdatedByEmail");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime")
                        .HasColumnName("UpdatedDate");

                    b.Property<string>("ZipCode")
                        .HasColumnType("varchar(5)")
                        .HasColumnName("ZipCode");

                    b.HasKey("RowId");

                    b.HasIndex("AccountId")
                        .IsUnique()
                        .HasFilter("[AccountId] IS NOT NULL");

                    b.HasIndex("UniqueId")
                        .IsUnique();

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("ELDEL_EntityFramework_Library.Models.Car", b =>
                {
                    b.Property<long>("RowId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("RowId")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedByEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime")
                        .HasColumnName("CreatedDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("Name");

                    b.Property<string>("RegistrationPlateId")
                        .IsRequired()
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("RegistrationPlateId");

                    b.Property<string>("UniqueId")
                        .IsRequired()
                        .IsUnicode(false)
                        .HasColumnType("char(15)")
                        .HasColumnName("UniqueId")
                        .IsFixedLength(true);

                    b.Property<string>("UpdatedByEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime")
                        .HasColumnName("UpdatedDate");

                    b.HasKey("RowId");

                    b.HasIndex("UniqueId")
                        .IsUnique();

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("ELDEL_EntityFramework_Library.Models.Charger", b =>
                {
                    b.Property<long>("RowId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("RowId")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("AddressRowId")
                        .HasColumnType("bigint")
                        .HasColumnName("AddressRowId");

                    b.Property<string>("AddressUniqueId")
                        .HasColumnType("char(15)")
                        .HasColumnName("AddressUniqueId");

                    b.Property<long?>("ChargerDetailsRowId")
                        .HasColumnType("bigint")
                        .HasColumnName("ChargerDetailsRowId");

                    b.Property<string>("ChargerDetailsUniqueId")
                        .HasColumnType("char(15)")
                        .HasColumnName("ChargerDetailsUniqueId");

                    b.Property<string>("CreatedByEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasColumnName("CreatedByEmail");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime")
                        .HasColumnName("CreatedDate");

                    b.Property<decimal?>("Latitude")
                        .HasColumnType("decimal(9,6)")
                        .HasColumnName("Latitude");

                    b.Property<decimal?>("Longitude")
                        .HasColumnType("decimal(9,6)")
                        .HasColumnName("Longitude");

                    b.Property<string>("ManufacturerType")
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("ManufacturerType");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(75)")
                        .HasColumnName("Name");

                    b.Property<string>("SocketType")
                        .HasColumnType("nvarchar(25)")
                        .HasColumnName("SocketType");

                    b.Property<string>("UniqueId")
                        .IsRequired()
                        .IsUnicode(false)
                        .HasColumnType("char(15)")
                        .HasColumnName("UniqueId")
                        .IsFixedLength(true);

                    b.Property<string>("UpdatedByEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasColumnName("UpdatedByEmail");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime")
                        .HasColumnName("UpdatedDate");

                    b.HasKey("RowId");

                    b.HasIndex("AddressRowId")
                        .IsUnique()
                        .HasFilter("[AddressRowId] IS NOT NULL");

                    b.HasIndex("UniqueId")
                        .IsUnique();

                    b.ToTable("Chargers");
                });

            modelBuilder.Entity("ELDEL_EntityFramework_Library.Models.ChargerDetails", b =>
                {
                    b.Property<long>("RowId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("RowId")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("ChargerRowId")
                        .HasColumnType("bigint")
                        .HasColumnName("ChargerRowId");

                    b.Property<string>("ChargerUniqueId")
                        .HasColumnType("char(15)")
                        .HasColumnName("ChargerUniqueId");

                    b.Property<string>("CreatedByEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasColumnName("CreatedByEmail");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime")
                        .HasColumnName("CreatedDate");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(256)")
                        .HasColumnName("Description");

                    b.Property<string>("UniqueId")
                        .IsRequired()
                        .IsUnicode(false)
                        .HasColumnType("char(15)")
                        .HasColumnName("UniqueId")
                        .IsFixedLength(true);

                    b.Property<string>("UpdatedByEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasColumnName("UpdatedByEmail");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime")
                        .HasColumnName("UpdatedDate");

                    b.HasKey("RowId");

                    b.HasIndex("ChargerRowId")
                        .IsUnique();

                    b.HasIndex("UniqueId")
                        .IsUnique();

                    b.ToTable("ChargerDetails");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("ELDEL_EntityFramework_Library.Models.AccountRole", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityRole");

                    b.Property<string>("CreatedByEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasColumnName("CreatedByEmail");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime")
                        .HasColumnName("CreatedDate");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleType")
                        .IsRequired()
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("RoleType");

                    b.Property<string>("UniqueId")
                        .IsRequired()
                        .IsUnicode(false)
                        .HasColumnType("char(15)")
                        .HasColumnName("UniqueId")
                        .IsFixedLength(true);

                    b.Property<string>("UpdatedByEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasColumnName("UpdatedByEmail");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime")
                        .HasColumnName("UpdatedDate");

                    b.HasIndex("UniqueId")
                        .IsUnique()
                        .HasFilter("[UniqueId] IS NOT NULL");

                    b.ToTable("AccountRoles");
                });

            modelBuilder.Entity("AccountCar", b =>
                {
                    b.HasOne("ELDEL_EntityFramework_Library.Models.Account", null)
                        .WithMany()
                        .HasForeignKey("AccountsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ELDEL_EntityFramework_Library.Models.Car", null)
                        .WithMany()
                        .HasForeignKey("CarsRowId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AccountCharger", b =>
                {
                    b.HasOne("ELDEL_EntityFramework_Library.Models.Account", null)
                        .WithMany()
                        .HasForeignKey("AccountsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ELDEL_EntityFramework_Library.Models.Charger", null)
                        .WithMany()
                        .HasForeignKey("ChargersRowId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ELDEL_EntityFramework_Library.Models.Address", b =>
                {
                    b.HasOne("ELDEL_EntityFramework_Library.Models.Account", "Account")
                        .WithOne("Address")
                        .HasForeignKey("ELDEL_EntityFramework_Library.Models.Address", "AccountId");

                    b.Navigation("Account");
                });

            modelBuilder.Entity("ELDEL_EntityFramework_Library.Models.Charger", b =>
                {
                    b.HasOne("ELDEL_EntityFramework_Library.Models.Address", "Address")
                        .WithOne("Charger")
                        .HasForeignKey("ELDEL_EntityFramework_Library.Models.Charger", "AddressRowId");

                    b.Navigation("Address");
                });

            modelBuilder.Entity("ELDEL_EntityFramework_Library.Models.ChargerDetails", b =>
                {
                    b.HasOne("ELDEL_EntityFramework_Library.Models.Charger", "Charger")
                        .WithOne("ChargerDetails")
                        .HasForeignKey("ELDEL_EntityFramework_Library.Models.ChargerDetails", "ChargerRowId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Charger");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ELDEL_EntityFramework_Library.Models.Account", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ELDEL_EntityFramework_Library.Models.Account", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ELDEL_EntityFramework_Library.Models.Account", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ELDEL_EntityFramework_Library.Models.Account", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ELDEL_EntityFramework_Library.Models.AccountRole", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithOne()
                        .HasForeignKey("ELDEL_EntityFramework_Library.Models.AccountRole", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ELDEL_EntityFramework_Library.Models.Account", b =>
                {
                    b.Navigation("Address");
                });

            modelBuilder.Entity("ELDEL_EntityFramework_Library.Models.Address", b =>
                {
                    b.Navigation("Charger");
                });

            modelBuilder.Entity("ELDEL_EntityFramework_Library.Models.Charger", b =>
                {
                    b.Navigation("ChargerDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
