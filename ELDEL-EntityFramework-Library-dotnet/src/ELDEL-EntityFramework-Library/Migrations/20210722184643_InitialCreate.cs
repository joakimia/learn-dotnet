using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ELDEL_EntityFramework_Library.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UniqueId = table.Column<string>(type: "char(15)", unicode: false, fixedLength: true, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(75)", unicode: false, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(75)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    PhoneNumberPrefix = table.Column<string>(type: "varchar(3)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedByEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressRowId = table.Column<long>(type: "bigint", nullable: true),
                    AddressUniqueId = table.Column<string>(type: "char(15)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    RowId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniqueId = table.Column<string>(type: "char(15)", unicode: false, fixedLength: true, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    RegistrationPlateId = table.Column<string>(type: "varchar(50)", unicode: false, nullable: false),
                    CreatedByEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedByEmail = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.RowId);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    RowId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniqueId = table.Column<string>(type: "char(15)", unicode: false, fixedLength: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    PrimaryStreetName = table.Column<string>(type: "nvarchar(75)", nullable: true),
                    SecondaryStreetName = table.Column<string>(type: "nvarchar(75)", nullable: true),
                    ZipCode = table.Column<string>(type: "varchar(5)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Province = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    CountryCode = table.Column<string>(type: "varchar(10)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedByEmail = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedByEmail = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AccountUniqueId = table.Column<string>(type: "char(15)", nullable: true),
                    ChargerRowId = table.Column<long>(type: "bigint", nullable: true),
                    ChargerUniqueId = table.Column<string>(type: "char(15)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.RowId);
                    table.ForeignKey(
                        name: "FK_Addresses_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_Accounts_UserId",
                        column: x => x.UserId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_Accounts_UserId",
                        column: x => x.UserId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_Accounts_UserId",
                        column: x => x.UserId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UniqueId = table.Column<string>(type: "char(15)", unicode: false, fixedLength: true, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleType = table.Column<string>(type: "varchar(50)", unicode: false, nullable: false),
                    CreatedByEmail = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedByEmail = table.Column<string>(type: "nvarchar(128)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountRoles_AspNetRoles_Id",
                        column: x => x.Id,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Accounts_UserId",
                        column: x => x.UserId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountCar",
                columns: table => new
                {
                    AccountsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CarsRowId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountCar", x => new { x.AccountsId, x.CarsRowId });
                    table.ForeignKey(
                        name: "FK_AccountCar_Accounts_AccountsId",
                        column: x => x.AccountsId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountCar_Cars_CarsRowId",
                        column: x => x.CarsRowId,
                        principalTable: "Cars",
                        principalColumn: "RowId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Chargers",
                columns: table => new
                {
                    RowId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniqueId = table.Column<string>(type: "char(15)", unicode: false, fixedLength: true, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(75)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(9,6)", nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(9,6)", nullable: true),
                    SocketType = table.Column<string>(type: "char(15)", nullable: true),
                    ManufacturerType = table.Column<string>(type: "char(50)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedByEmail = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedByEmail = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    AddressRowId = table.Column<long>(type: "bigint", nullable: true),
                    AddressUniqueId = table.Column<string>(type: "char(15)", nullable: true),
                    ChargerDetailsRowId = table.Column<long>(type: "bigint", nullable: true),
                    ChargerDetailsUniqueId = table.Column<string>(type: "char(15)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chargers", x => x.RowId);
                    table.ForeignKey(
                        name: "FK_Chargers_Addresses_AddressRowId",
                        column: x => x.AddressRowId,
                        principalTable: "Addresses",
                        principalColumn: "RowId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccountCharger",
                columns: table => new
                {
                    AccountsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ChargersRowId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountCharger", x => new { x.AccountsId, x.ChargersRowId });
                    table.ForeignKey(
                        name: "FK_AccountCharger_Accounts_AccountsId",
                        column: x => x.AccountsId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountCharger_Chargers_ChargersRowId",
                        column: x => x.ChargersRowId,
                        principalTable: "Chargers",
                        principalColumn: "RowId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChargerDetails",
                columns: table => new
                {
                    RowId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniqueId = table.Column<string>(type: "char(15)", unicode: false, fixedLength: true, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    CreatedByEmail = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedByEmail = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    ChargerRowId = table.Column<long>(type: "bigint", nullable: false),
                    ChargerUniqueId = table.Column<string>(type: "char(15)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChargerDetails", x => x.RowId);
                    table.ForeignKey(
                        name: "FK_ChargerDetails_Chargers_ChargerRowId",
                        column: x => x.ChargerRowId,
                        principalTable: "Chargers",
                        principalColumn: "RowId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountCar_CarsRowId",
                table: "AccountCar",
                column: "CarsRowId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountCharger_ChargersRowId",
                table: "AccountCharger",
                column: "ChargersRowId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountRoles_UniqueId",
                table: "AccountRoles",
                column: "UniqueId",
                unique: true,
                filter: "[UniqueId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Accounts",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UniqueId",
                table: "Accounts",
                column: "UniqueId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Accounts",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_AccountId",
                table: "Addresses",
                column: "AccountId",
                unique: true,
                filter: "[AccountId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UniqueId",
                table: "Addresses",
                column: "UniqueId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_UniqueId",
                table: "Cars",
                column: "UniqueId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChargerDetails_ChargerRowId",
                table: "ChargerDetails",
                column: "ChargerRowId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChargerDetails_UniqueId",
                table: "ChargerDetails",
                column: "UniqueId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Chargers_AddressRowId",
                table: "Chargers",
                column: "AddressRowId",
                unique: true,
                filter: "[AddressRowId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Chargers_UniqueId",
                table: "Chargers",
                column: "UniqueId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountCar");

            migrationBuilder.DropTable(
                name: "AccountCharger");

            migrationBuilder.DropTable(
                name: "AccountRoles");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ChargerDetails");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Chargers");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
