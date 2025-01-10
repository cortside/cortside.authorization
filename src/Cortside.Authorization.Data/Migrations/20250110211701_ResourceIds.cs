using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cortside.Authorization.Data.Migrations
{
    /// <inheritdoc />
    public partial class ResourceIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "dbo",
                table: "PolicyRoleClaim",
                newName: "PolicyRoleClaimId");

            migrationBuilder.AddColumn<Guid>(
                name: "RolePermissionResourceId",
                schema: "dbo",
                table: "RolePermission",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Public unique identifier");

            migrationBuilder.AddColumn<Guid>(
                name: "RoleResourceId",
                schema: "dbo",
                table: "Role",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Public unique identifier");

            migrationBuilder.AddColumn<Guid>(
                name: "PolicyRoleClaimResourceId",
                schema: "dbo",
                table: "PolicyRoleClaim",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Public unique identifier");

            migrationBuilder.AddColumn<Guid>(
                name: "PolicyResourceId",
                schema: "dbo",
                table: "Policy",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Public unique identifier");

            migrationBuilder.AddColumn<Guid>(
                name: "PermissionResourceId",
                schema: "dbo",
                table: "Permission",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Public unique identifier");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_RolePermissionResourceId",
                schema: "dbo",
                table: "RolePermission",
                column: "RolePermissionResourceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Role_RoleResourceId",
                schema: "dbo",
                table: "Role",
                column: "RoleResourceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PolicyRoleClaim_ClaimType_Value",
                schema: "dbo",
                table: "PolicyRoleClaim",
                columns: new[] { "ClaimType", "Value" });

            migrationBuilder.CreateIndex(
                name: "IX_PolicyRoleClaim_PolicyRoleClaimResourceId",
                schema: "dbo",
                table: "PolicyRoleClaim",
                column: "PolicyRoleClaimResourceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Policy_Name",
                schema: "dbo",
                table: "Policy",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Policy_PolicyResourceId",
                schema: "dbo",
                table: "Policy",
                column: "PolicyResourceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permission_PermissionResourceId",
                schema: "dbo",
                table: "Permission",
                column: "PermissionResourceId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RolePermission_RolePermissionResourceId",
                schema: "dbo",
                table: "RolePermission");

            migrationBuilder.DropIndex(
                name: "IX_Role_RoleResourceId",
                schema: "dbo",
                table: "Role");

            migrationBuilder.DropIndex(
                name: "IX_PolicyRoleClaim_ClaimType_Value",
                schema: "dbo",
                table: "PolicyRoleClaim");

            migrationBuilder.DropIndex(
                name: "IX_PolicyRoleClaim_PolicyRoleClaimResourceId",
                schema: "dbo",
                table: "PolicyRoleClaim");

            migrationBuilder.DropIndex(
                name: "IX_Policy_Name",
                schema: "dbo",
                table: "Policy");

            migrationBuilder.DropIndex(
                name: "IX_Policy_PolicyResourceId",
                schema: "dbo",
                table: "Policy");

            migrationBuilder.DropIndex(
                name: "IX_Permission_PermissionResourceId",
                schema: "dbo",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "RolePermissionResourceId",
                schema: "dbo",
                table: "RolePermission");

            migrationBuilder.DropColumn(
                name: "RoleResourceId",
                schema: "dbo",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "PolicyRoleClaimResourceId",
                schema: "dbo",
                table: "PolicyRoleClaim");

            migrationBuilder.DropColumn(
                name: "PolicyResourceId",
                schema: "dbo",
                table: "Policy");

            migrationBuilder.DropColumn(
                name: "PermissionResourceId",
                schema: "dbo",
                table: "Permission");

            migrationBuilder.RenameColumn(
                name: "PolicyRoleClaimId",
                schema: "dbo",
                table: "PolicyRoleClaim",
                newName: "Id");
        }
    }
}
