using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class sqlServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Forms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false),
                    Description = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false),
                    Description = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false),
                    Description = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "varchar(100)", nullable: false),
                    LastName = table.Column<string>(type: "varchar(100)", nullable: false),
                    Identification = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(20)", nullable: false),
                    Address = table.Column<string>(type: "varchar(100)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TokenHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    ReplacedByTokenHash = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rols",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false),
                    Description = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rols", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormModules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormId = table.Column<int>(type: "int", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormModules_Forms_FormId",
                        column: x => x.FormId,
                        principalTable: "Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormModules_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Password = table.Column<string>(type: "varchar(100)", nullable: true),
                    Email = table.Column<string>(type: "varchar(150)", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolFormPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RolId = table.Column<int>(type: "int", nullable: false),
                    FormId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolFormPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolFormPermissions_Forms_FormId",
                        column: x => x.FormId,
                        principalTable: "Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolFormPermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolFormPermissions_Rols_RolId",
                        column: x => x.RolId,
                        principalTable: "Rols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RolId = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolUsers_Rols_RolId",
                        column: x => x.RolId,
                        principalTable: "Rols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Forms",
                columns: new[] { "Id", "Active", "CreatedAt", "Description", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Form Update User", false, "Form User" },
                    { 2, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Form Create Rol", false, "Form Rol" }
                });

            migrationBuilder.InsertData(
                table: "Modules",
                columns: new[] { "Id", "Active", "CreatedAt", "Description", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Module for User", false, "Module User" },
                    { 2, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Module for Rol", false, "Module Rol" }
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Active", "CreatedAt", "Description", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Permiso de creacion", false, "Crear" },
                    { 2, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Permiso de borrar", false, "Borrar" },
                    { 3, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Permiso de Actualizar", false, "Actualizar" },
                    { 4, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Permiso de Leer", false, "Leer" }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Active", "Address", "CreatedAt", "FirstName", "Identification", "IsDeleted", "LastName", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, true, "AV SiempreViva", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Admin", "0000000000", false, "Admin", "1234567890" },
                    { 2, true, "AV SiempreViva", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "User", "1111111111", false, "User", "1234567890" }
                });

            migrationBuilder.InsertData(
                table: "Rols",
                columns: new[] { "Id", "Active", "CreatedAt", "Description", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Rol con permisos administrativos", false, "Administrador" },
                    { 2, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Rol con permisos de usuario", false, "Usuario" }
                });

            migrationBuilder.InsertData(
                table: "FormModules",
                columns: new[] { "Id", "Active", "CreatedAt", "FormId", "IsDeleted", "ModuleId" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1 },
                    { 2, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, 2 }
                });

            migrationBuilder.InsertData(
                table: "RolFormPermissions",
                columns: new[] { "Id", "Active", "CreatedAt", "FormId", "IsDeleted", "PermissionId", "RolId" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, 1, 1 },
                    { 2, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, 2, 2 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Active", "CreatedAt", "Email", "IsDeleted", "Password", "PersonId" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@example.com", false, "admin123", 1 },
                    { 2, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "User@example.com", false, "user123", 2 }
                });

            migrationBuilder.InsertData(
                table: "RolUsers",
                columns: new[] { "Id", "Active", "CreatedAt", "IsDeleted", "RolId", "UserId" },
                values: new object[] { 1, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_FormModules_FormId",
                table: "FormModules",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_FormModules_ModuleId",
                table: "FormModules",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_RolFormPermissions_FormId",
                table: "RolFormPermissions",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_RolFormPermissions_PermissionId",
                table: "RolFormPermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolFormPermissions_RolId",
                table: "RolFormPermissions",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_RolUsers_RolId",
                table: "RolUsers",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_RolUsers_UserId",
                table: "RolUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PersonId",
                table: "Users",
                column: "PersonId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormModules");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "RolFormPermissions");

            migrationBuilder.DropTable(
                name: "RolUsers");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropTable(
                name: "Forms");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Rols");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}
