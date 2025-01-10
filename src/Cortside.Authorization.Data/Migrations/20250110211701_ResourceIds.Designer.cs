﻿// <auto-generated />
using System;
using Cortside.Authorization.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Cortside.Authorization.Data.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20250110211701_ResourceIds")]
    partial class ResourceIds
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("dbo")
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Cortside.AspNetCore.Auditable.Entities.Subject", b =>
                {
                    b.Property<Guid>("SubjectId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Subject primary key");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2")
                        .HasComment("Date and time entity was created");

                    b.Property<string>("FamilyName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasComment("Subject Surname ()");

                    b.Property<string>("GivenName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasComment("Subject primary key");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasComment("Subject primary key");

                    b.Property<string>("UserPrincipalName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasComment("Username (upn claim)");

                    b.HasKey("SubjectId");

                    b.ToTable("Subject", "dbo", t =>
                        {
                            t.HasTrigger("trSubject");
                        });

                    b.HasAnnotation("SqlServer:UseSqlOutputClause", false);
                });

            modelBuilder.Entity("Cortside.Authorization.Domain.Entities.Permission", b =>
                {
                    b.Property<int>("PermissionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Auto incrementing id that is for internal use only");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PermissionId"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2")
                        .HasComment("Date and time entity was created");

                    b.Property<Guid>("CreatedSubjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(255)")
                        .HasComment("Permission description");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("datetime2")
                        .HasComment("Date and time entity was last modified");

                    b.Property<Guid>("LastModifiedSubjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(100)")
                        .HasComment("Name of the Permission");

                    b.Property<Guid>("PermissionResourceId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Public unique identifier");

                    b.Property<int>("PolicyId")
                        .HasColumnType("int")
                        .HasComment("FK to Policy");

                    b.HasKey("PermissionId");

                    b.HasIndex("CreatedSubjectId");

                    b.HasIndex("LastModifiedSubjectId");

                    b.HasIndex("PermissionResourceId")
                        .IsUnique();

                    b.HasIndex("PolicyId");

                    b.ToTable("Permission", "dbo", t =>
                        {
                            t.HasComment("Permissions available within a policy");

                            t.HasTrigger("trPermission");
                        });

                    b.HasAnnotation("SqlServer:UseSqlOutputClause", false);
                });

            modelBuilder.Entity("Cortside.Authorization.Domain.Entities.Policy", b =>
                {
                    b.Property<int>("PolicyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Auto incrementing id that is for internal use only");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PolicyId"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2")
                        .HasComment("Date and time entity was created");

                    b.Property<Guid>("CreatedSubjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(255)")
                        .HasComment("Policy description");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("datetime2")
                        .HasComment("Date and time entity was last modified");

                    b.Property<Guid>("LastModifiedSubjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(100)")
                        .HasComment("Name of the policy");

                    b.Property<Guid>("PolicyResourceId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Public unique identifier");

                    b.HasKey("PolicyId");

                    b.HasIndex("CreatedSubjectId");

                    b.HasIndex("LastModifiedSubjectId");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.HasIndex("PolicyResourceId")
                        .IsUnique();

                    b.ToTable("Policy", "dbo", t =>
                        {
                            t.HasComment("Policies of the application");

                            t.HasTrigger("trPolicy");
                        });

                    b.HasAnnotation("SqlServer:UseSqlOutputClause", false);
                });

            modelBuilder.Entity("Cortside.Authorization.Domain.Entities.PolicyRoleClaim", b =>
                {
                    b.Property<int>("PolicyRoleClaimId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Auto incrementing id that is for internal use only");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PolicyRoleClaimId"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(100)")
                        .HasComment("ClaimType of the PolicyRoleClaim, i.e. sub/role/group etc");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2")
                        .HasComment("Date and time entity was created");

                    b.Property<Guid>("CreatedSubjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(255)")
                        .HasComment("PolicyRoleClaim description");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("datetime2")
                        .HasComment("Date and time entity was last modified");

                    b.Property<Guid>("LastModifiedSubjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PolicyRoleClaimResourceId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Public unique identifier");

                    b.Property<int>("RoleId")
                        .HasColumnType("int")
                        .HasComment("FK to Role");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(500)")
                        .HasComment("Value of the claim");

                    b.HasKey("PolicyRoleClaimId");

                    b.HasIndex("CreatedSubjectId");

                    b.HasIndex("LastModifiedSubjectId");

                    b.HasIndex("PolicyRoleClaimResourceId")
                        .IsUnique();

                    b.HasIndex("RoleId");

                    b.HasIndex("ClaimType", "Value");

                    b.ToTable("PolicyRoleClaim", "dbo", t =>
                        {
                            t.HasComment("PolicyRoleClaims within a role");

                            t.HasTrigger("trPolicyRoleClaim");
                        });

                    b.HasAnnotation("SqlServer:UseSqlOutputClause", false);
                });

            modelBuilder.Entity("Cortside.Authorization.Domain.Entities.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Auto incrementing id that is for internal use only");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2")
                        .HasComment("Date and time entity was created");

                    b.Property<Guid>("CreatedSubjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(255)")
                        .HasComment("Role description");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("datetime2")
                        .HasComment("Date and time entity was last modified");

                    b.Property<Guid>("LastModifiedSubjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(100)")
                        .HasComment("Name of the Role");

                    b.Property<int>("PolicyId")
                        .HasColumnType("int")
                        .HasComment("FK to Policy");

                    b.Property<Guid>("RoleResourceId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Public unique identifier");

                    b.HasKey("RoleId");

                    b.HasIndex("CreatedSubjectId");

                    b.HasIndex("LastModifiedSubjectId");

                    b.HasIndex("PolicyId");

                    b.HasIndex("RoleResourceId")
                        .IsUnique();

                    b.ToTable("Role", "dbo", t =>
                        {
                            t.HasComment("Roles within a policy");

                            t.HasTrigger("trRole");
                        });

                    b.HasAnnotation("SqlServer:UseSqlOutputClause", false);
                });

            modelBuilder.Entity("Cortside.Authorization.Domain.Entities.RolePermission", b =>
                {
                    b.Property<int>("RolePermissionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Auto incrementing id that is for internal use only");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RolePermissionId"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2")
                        .HasComment("Date and time entity was created");

                    b.Property<Guid>("CreatedSubjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("datetime2")
                        .HasComment("Date and time entity was last modified");

                    b.Property<Guid>("LastModifiedSubjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("PermissionId")
                        .HasColumnType("int")
                        .HasComment("FK to Permission");

                    b.Property<int>("RoleId")
                        .HasColumnType("int")
                        .HasComment("FK to Role");

                    b.Property<Guid>("RolePermissionResourceId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Public unique identifier");

                    b.HasKey("RolePermissionId");

                    b.HasIndex("CreatedSubjectId");

                    b.HasIndex("LastModifiedSubjectId");

                    b.HasIndex("PermissionId");

                    b.HasIndex("RoleId");

                    b.HasIndex("RolePermissionResourceId")
                        .IsUnique();

                    b.ToTable("RolePermission", "dbo", t =>
                        {
                            t.HasComment("RolePermissions are permissions assigned to a role within a policy");

                            t.HasTrigger("trRolePermission");
                        });

                    b.HasAnnotation("SqlServer:UseSqlOutputClause", false);
                });

            modelBuilder.Entity("Cortside.Authorization.Domain.Entities.Permission", b =>
                {
                    b.HasOne("Cortside.AspNetCore.Auditable.Entities.Subject", "CreatedSubject")
                        .WithMany()
                        .HasForeignKey("CreatedSubjectId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Cortside.AspNetCore.Auditable.Entities.Subject", "LastModifiedSubject")
                        .WithMany()
                        .HasForeignKey("LastModifiedSubjectId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Cortside.Authorization.Domain.Entities.Policy", "Policy")
                        .WithMany("Permissions")
                        .HasForeignKey("PolicyId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("CreatedSubject");

                    b.Navigation("LastModifiedSubject");

                    b.Navigation("Policy");
                });

            modelBuilder.Entity("Cortside.Authorization.Domain.Entities.Policy", b =>
                {
                    b.HasOne("Cortside.AspNetCore.Auditable.Entities.Subject", "CreatedSubject")
                        .WithMany()
                        .HasForeignKey("CreatedSubjectId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Cortside.AspNetCore.Auditable.Entities.Subject", "LastModifiedSubject")
                        .WithMany()
                        .HasForeignKey("LastModifiedSubjectId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("CreatedSubject");

                    b.Navigation("LastModifiedSubject");
                });

            modelBuilder.Entity("Cortside.Authorization.Domain.Entities.PolicyRoleClaim", b =>
                {
                    b.HasOne("Cortside.AspNetCore.Auditable.Entities.Subject", "CreatedSubject")
                        .WithMany()
                        .HasForeignKey("CreatedSubjectId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Cortside.AspNetCore.Auditable.Entities.Subject", "LastModifiedSubject")
                        .WithMany()
                        .HasForeignKey("LastModifiedSubjectId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Cortside.Authorization.Domain.Entities.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("CreatedSubject");

                    b.Navigation("LastModifiedSubject");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Cortside.Authorization.Domain.Entities.Role", b =>
                {
                    b.HasOne("Cortside.AspNetCore.Auditable.Entities.Subject", "CreatedSubject")
                        .WithMany()
                        .HasForeignKey("CreatedSubjectId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Cortside.AspNetCore.Auditable.Entities.Subject", "LastModifiedSubject")
                        .WithMany()
                        .HasForeignKey("LastModifiedSubjectId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Cortside.Authorization.Domain.Entities.Policy", "Policy")
                        .WithMany("Roles")
                        .HasForeignKey("PolicyId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("CreatedSubject");

                    b.Navigation("LastModifiedSubject");

                    b.Navigation("Policy");
                });

            modelBuilder.Entity("Cortside.Authorization.Domain.Entities.RolePermission", b =>
                {
                    b.HasOne("Cortside.AspNetCore.Auditable.Entities.Subject", "CreatedSubject")
                        .WithMany()
                        .HasForeignKey("CreatedSubjectId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Cortside.AspNetCore.Auditable.Entities.Subject", "LastModifiedSubject")
                        .WithMany()
                        .HasForeignKey("LastModifiedSubjectId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Cortside.Authorization.Domain.Entities.Permission", "Permission")
                        .WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Cortside.Authorization.Domain.Entities.Role", "Role")
                        .WithMany("RolePermissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("CreatedSubject");

                    b.Navigation("LastModifiedSubject");

                    b.Navigation("Permission");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Cortside.Authorization.Domain.Entities.Policy", b =>
                {
                    b.Navigation("Permissions");

                    b.Navigation("Roles");
                });

            modelBuilder.Entity("Cortside.Authorization.Domain.Entities.Role", b =>
                {
                    b.Navigation("RolePermissions");
                });
#pragma warning restore 612, 618
        }
    }
}
