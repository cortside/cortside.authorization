using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cortside.Authorization.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Subject",
                schema: "dbo",
                columns: table => new
                {
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Subject primary key"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Subject primary key"),
                    GivenName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Subject primary key"),
                    FamilyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Subject Surname ()"),
                    UserPrincipalName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Username (upn claim)"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date and time entity was created")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subject", x => x.SubjectId);
                });

            migrationBuilder.CreateTable(
                name: "Policy",
                schema: "dbo",
                columns: table => new
                {
                    PolicyId = table.Column<int>(type: "int", nullable: false, comment: "Auto incrementing id that is for internal use only")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: true, comment: "Name of the policy"),
                    Description = table.Column<string>(type: "nvarchar(255)", nullable: true, comment: "Policy description"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date and time entity was created"),
                    CreatedSubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date and time entity was last modified"),
                    LastModifiedSubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Policy", x => x.PolicyId);
                    table.ForeignKey(
                        name: "FK_Policy_Subject_CreatedSubjectId",
                        column: x => x.CreatedSubjectId,
                        principalSchema: "dbo",
                        principalTable: "Subject",
                        principalColumn: "SubjectId");
                    table.ForeignKey(
                        name: "FK_Policy_Subject_LastModifiedSubjectId",
                        column: x => x.LastModifiedSubjectId,
                        principalSchema: "dbo",
                        principalTable: "Subject",
                        principalColumn: "SubjectId");
                },
                comment: "Policies of the application");

            migrationBuilder.CreateTable(
                name: "Permission",
                schema: "dbo",
                columns: table => new
                {
                    PermissionId = table.Column<int>(type: "int", nullable: false, comment: "Auto incrementing id that is for internal use only")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: true, comment: "Name of the Permission"),
                    Description = table.Column<string>(type: "nvarchar(255)", nullable: true, comment: "Permission description"),
                    PolicyId = table.Column<int>(type: "int", nullable: false, comment: "FK to Policy"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date and time entity was created"),
                    CreatedSubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date and time entity was last modified"),
                    LastModifiedSubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.PermissionId);
                    table.ForeignKey(
                        name: "FK_Permission_Policy_PolicyId",
                        column: x => x.PolicyId,
                        principalSchema: "dbo",
                        principalTable: "Policy",
                        principalColumn: "PolicyId");
                    table.ForeignKey(
                        name: "FK_Permission_Subject_CreatedSubjectId",
                        column: x => x.CreatedSubjectId,
                        principalSchema: "dbo",
                        principalTable: "Subject",
                        principalColumn: "SubjectId");
                    table.ForeignKey(
                        name: "FK_Permission_Subject_LastModifiedSubjectId",
                        column: x => x.LastModifiedSubjectId,
                        principalSchema: "dbo",
                        principalTable: "Subject",
                        principalColumn: "SubjectId");
                },
                comment: "Permissions available within a policy");

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "dbo",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false, comment: "Auto incrementing id that is for internal use only")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: true, comment: "Name of the Role"),
                    Description = table.Column<string>(type: "nvarchar(255)", nullable: true, comment: "Role description"),
                    PolicyId = table.Column<int>(type: "int", nullable: false, comment: "FK to Policy"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date and time entity was created"),
                    CreatedSubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date and time entity was last modified"),
                    LastModifiedSubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                    table.ForeignKey(
                        name: "FK_Role_Policy_PolicyId",
                        column: x => x.PolicyId,
                        principalSchema: "dbo",
                        principalTable: "Policy",
                        principalColumn: "PolicyId");
                    table.ForeignKey(
                        name: "FK_Role_Subject_CreatedSubjectId",
                        column: x => x.CreatedSubjectId,
                        principalSchema: "dbo",
                        principalTable: "Subject",
                        principalColumn: "SubjectId");
                    table.ForeignKey(
                        name: "FK_Role_Subject_LastModifiedSubjectId",
                        column: x => x.LastModifiedSubjectId,
                        principalSchema: "dbo",
                        principalTable: "Subject",
                        principalColumn: "SubjectId");
                },
                comment: "Roles within a policy");

            migrationBuilder.CreateTable(
                name: "PolicyRoleClaim",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Auto incrementing id that is for internal use only")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimType = table.Column<string>(type: "nvarchar(100)", nullable: true, comment: "ClaimType of the PolicyRoleClaim, i.e. sub/role/group etc"),
                    Value = table.Column<string>(type: "nvarchar(500)", nullable: true, comment: "Value of the claim"),
                    Description = table.Column<string>(type: "nvarchar(255)", nullable: true, comment: "PolicyRoleClaim description"),
                    RoleId = table.Column<int>(type: "int", nullable: false, comment: "FK to Role"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date and time entity was created"),
                    CreatedSubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date and time entity was last modified"),
                    LastModifiedSubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolicyRoleClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PolicyRoleClaim_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "Role",
                        principalColumn: "RoleId");
                    table.ForeignKey(
                        name: "FK_PolicyRoleClaim_Subject_CreatedSubjectId",
                        column: x => x.CreatedSubjectId,
                        principalSchema: "dbo",
                        principalTable: "Subject",
                        principalColumn: "SubjectId");
                    table.ForeignKey(
                        name: "FK_PolicyRoleClaim_Subject_LastModifiedSubjectId",
                        column: x => x.LastModifiedSubjectId,
                        principalSchema: "dbo",
                        principalTable: "Subject",
                        principalColumn: "SubjectId");
                },
                comment: "PolicyRoleClaims within a role");

            migrationBuilder.CreateTable(
                name: "RolePermission",
                schema: "dbo",
                columns: table => new
                {
                    RolePermissionId = table.Column<int>(type: "int", nullable: false, comment: "Auto incrementing id that is for internal use only")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false, comment: "FK to Role"),
                    PermissionId = table.Column<int>(type: "int", nullable: false, comment: "FK to Permission"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date and time entity was created"),
                    CreatedSubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date and time entity was last modified"),
                    LastModifiedSubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => x.RolePermissionId);
                    table.ForeignKey(
                        name: "FK_RolePermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "dbo",
                        principalTable: "Permission",
                        principalColumn: "PermissionId");
                    table.ForeignKey(
                        name: "FK_RolePermission_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "Role",
                        principalColumn: "RoleId");
                    table.ForeignKey(
                        name: "FK_RolePermission_Subject_CreatedSubjectId",
                        column: x => x.CreatedSubjectId,
                        principalSchema: "dbo",
                        principalTable: "Subject",
                        principalColumn: "SubjectId");
                    table.ForeignKey(
                        name: "FK_RolePermission_Subject_LastModifiedSubjectId",
                        column: x => x.LastModifiedSubjectId,
                        principalSchema: "dbo",
                        principalTable: "Subject",
                        principalColumn: "SubjectId");
                },
                comment: "RolePermissions are permissions assigned to a role within a policy");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_CreatedSubjectId",
                schema: "dbo",
                table: "Permission",
                column: "CreatedSubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_LastModifiedSubjectId",
                schema: "dbo",
                table: "Permission",
                column: "LastModifiedSubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_PolicyId",
                schema: "dbo",
                table: "Permission",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_Policy_CreatedSubjectId",
                schema: "dbo",
                table: "Policy",
                column: "CreatedSubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Policy_LastModifiedSubjectId",
                schema: "dbo",
                table: "Policy",
                column: "LastModifiedSubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PolicyRoleClaim_CreatedSubjectId",
                schema: "dbo",
                table: "PolicyRoleClaim",
                column: "CreatedSubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PolicyRoleClaim_LastModifiedSubjectId",
                schema: "dbo",
                table: "PolicyRoleClaim",
                column: "LastModifiedSubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PolicyRoleClaim_RoleId",
                schema: "dbo",
                table: "PolicyRoleClaim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_CreatedSubjectId",
                schema: "dbo",
                table: "Role",
                column: "CreatedSubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_LastModifiedSubjectId",
                schema: "dbo",
                table: "Role",
                column: "LastModifiedSubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_PolicyId",
                schema: "dbo",
                table: "Role",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_CreatedSubjectId",
                schema: "dbo",
                table: "RolePermission",
                column: "CreatedSubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_LastModifiedSubjectId",
                schema: "dbo",
                table: "RolePermission",
                column: "LastModifiedSubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_PermissionId",
                schema: "dbo",
                table: "RolePermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_RoleId",
                schema: "dbo",
                table: "RolePermission",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PolicyRoleClaim",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RolePermission",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Permission",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Policy",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Subject",
                schema: "dbo");
        }
    }
}
