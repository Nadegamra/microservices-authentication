﻿// <auto-generated />
using System;
using Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Authentication.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230601105236_EmailAndPasswordTokens")]
    partial class EmailAndPasswordTokens
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Authentication.Models.EmailChangeToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("EmailChangeTokens");
                });

            modelBuilder.Entity("Authentication.Models.EmailConfirmationToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("EmailConfirmationTokens");
                });

            modelBuilder.Entity("Authentication.Models.PasswordChangeToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("PasswordChangeTokens");
                });

            modelBuilder.Entity("Authentication.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("NormalizedName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = 2,
                            Name = "creator",
                            NormalizedName = "CREATOR"
                        },
                        new
                        {
                            Id = 3,
                            Name = "consumer",
                            NormalizedName = "CONSUMER"
                        });
                });

            modelBuilder.Entity("Authentication.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("NormalizedEmail")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("NormalizedUsername")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "admin@admin.com",
                            EmailConfirmed = true,
                            NormalizedEmail = "ADMIN@ADMIN.COM",
                            NormalizedUsername = "ADMIN@ADMIN.COM",
                            PasswordHash = "0E63C20429D349EEED0C689DB2E47F1661927CFFFB8124DA456276854575367D0DED966F2C00D9D3A162B98DB8915371A880FB079C86E2AF9C04CC751A9BB9174FF0913A71ABD5CBE112EEAEC812709DB749234CF9ADDECE2937F2A1FC0BF2554E3EA3655EBFB712E0598B45C05FBF893084C7AA1ABF1F990603DD1D9B71400A",
                            Username = "admin@admin.com"
                        },
                        new
                        {
                            Id = 2,
                            Email = "creator@example.com",
                            EmailConfirmed = true,
                            NormalizedEmail = "CREATOR@EXAMPLE.COM",
                            NormalizedUsername = "CREATOR@EXAMPLE.COM",
                            PasswordHash = "0E63C20429D349EEED0C689DB2E47F1661927CFFFB8124DA456276854575367D0DED966F2C00D9D3A162B98DB8915371A880FB079C86E2AF9C04CC751A9BB9174FF0913A71ABD5CBE112EEAEC812709DB749234CF9ADDECE2937F2A1FC0BF2554E3EA3655EBFB712E0598B45C05FBF893084C7AA1ABF1F990603DD1D9B71400A",
                            Username = "creator@example.com"
                        },
                        new
                        {
                            Id = 3,
                            Email = "consumer@example.com",
                            EmailConfirmed = true,
                            NormalizedEmail = "CONSUMER@EXAMPLE.COM",
                            NormalizedUsername = "CONSUMER@EXAMPLE.COM",
                            PasswordHash = "0E63C20429D349EEED0C689DB2E47F1661927CFFFB8124DA456276854575367D0DED966F2C00D9D3A162B98DB8915371A880FB079C86E2AF9C04CC751A9BB9174FF0913A71ABD5CBE112EEAEC812709DB749234CF9ADDECE2937F2A1FC0BF2554E3EA3655EBFB712E0598B45C05FBF893084C7AA1ABF1F990603DD1D9B71400A",
                            Username = "consumer@example.com"
                        });
                });

            modelBuilder.Entity("Authentication.Models.UserRole", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.ToTable("UserRoles");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            RoleId = 1
                        },
                        new
                        {
                            UserId = 2,
                            RoleId = 2
                        },
                        new
                        {
                            UserId = 3,
                            RoleId = 3
                        });
                });

            modelBuilder.Entity("Authentication.Models.UserToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("AccessExpiry")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("AccessToken")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("RefreshExpiry")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("UserTokens");
                });
#pragma warning restore 612, 618
        }
    }
}
