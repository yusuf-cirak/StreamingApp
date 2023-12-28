﻿// <auto-generated />
using System;
using Infrastructure.Persistence.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Persistence.EntityFramework.Migrations
{
    [DbContext(typeof(BaseDbContext))]
    partial class BaseDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Application.Common.Models.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<string>("Error")
                        .HasColumnType("text");

                    b.Property<DateTime>("OccuredOnUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("ProcessedOnUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Type")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("OutboxMessages");
                });

            modelBuilder.Entity("Domain.Entities.OperationClaim", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("Id");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("Name");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("OperationClaims", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("Id");

                    b.Property<string>("CreatedByIp")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("CreatedByIp");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("ExpiresAt");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Token");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("UserId");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("Id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Name");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Roles", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.RoleOperationClaim", b =>
                {
                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid")
                        .HasColumnName("RoleId");

                    b.Property<Guid>("OperationClaimId")
                        .HasColumnType("uuid")
                        .HasColumnName("OperationClaimId");

                    b.HasKey("RoleId", "OperationClaimId");

                    b.HasIndex("OperationClaimId")
                        .IsUnique();

                    b.HasIndex("RoleId");

                    b.HasIndex("RoleId", "OperationClaimId")
                        .IsUnique();

                    b.ToTable("RoleOperationClaims", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Stream", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("Id");

                    b.Property<int>("ChatDelaySecond")
                        .HasColumnType("integer")
                        .HasColumnName("ChatDelaySecond");

                    b.Property<bool>("ChatDisabled")
                        .HasColumnType("boolean")
                        .HasColumnName("ChatDisabled");

                    b.Property<DateTime?>("EndedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("EndedAt");

                    b.Property<DateTime>("StartedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("StartedAt");

                    b.Property<Guid>("StreamerId")
                        .HasColumnType("uuid")
                        .HasColumnName("StreamerId");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("StreamerId")
                        .IsUnique();

                    b.ToTable("Streams", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.StreamBlockedUser", b =>
                {
                    b.Property<Guid>("StreamerId")
                        .HasColumnType("uuid")
                        .HasColumnName("StreamerId");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("UserId");

                    b.HasKey("StreamerId", "UserId");

                    b.HasIndex("UserId");

                    b.HasIndex("StreamerId", "UserId")
                        .IsUnique();

                    b.ToTable("StreamBlockedUsers", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.StreamChatMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("Id");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CreatedAt");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Message");

                    b.Property<Guid>("StreamId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("StreamerId")
                        .HasColumnType("uuid")
                        .HasColumnName("StreamerId");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("StreamerId")
                        .IsUnique();

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("StreamChatMessages", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.StreamFollowerUser", b =>
                {
                    b.Property<Guid>("StreamerId")
                        .HasColumnType("uuid")
                        .HasColumnName("StreamerId");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("UserId");

                    b.HasKey("StreamerId", "UserId");

                    b.HasIndex("StreamerId");

                    b.HasIndex("UserId");

                    b.HasIndex("StreamerId", "UserId")
                        .IsUnique();

                    b.ToTable("StreamFollowerUsers", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Streamer", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("Id");

                    b.Property<string>("StreamDescription")
                        .HasColumnType("text")
                        .HasColumnName("StreamDescription");

                    b.Property<string>("StreamKey")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("StreamKey");

                    b.Property<string>("StreamTitle")
                        .HasColumnType("text")
                        .HasColumnName("StreamTitle");

                    b.HasKey("Id");

                    b.ToTable("Streamers", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("Id");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("CreatedDate");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("PasswordHash");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("PasswordSalt");

                    b.Property<string>("ProfileImageUrl")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("UpdatedDate");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Username");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.UserOperationClaim", b =>
                {
                    b.Property<Guid>("OperationClaimId")
                        .HasColumnType("uuid")
                        .HasColumnName("OperationClaimId");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("UserId");

                    b.Property<string>("Value")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("OperationClaimId", "UserId", "Value");

                    b.HasIndex("OperationClaimId");

                    b.HasIndex("UserId");

                    b.HasIndex("OperationClaimId", "UserId", "Value")
                        .IsUnique();

                    b.ToTable("UserOperationClaims", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.UserRoleClaim", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("UserId");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid")
                        .HasColumnName("RoleId");

                    b.Property<string>("Value")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("UserId", "RoleId", "Value");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.HasIndex("UserId", "RoleId", "Value")
                        .IsUnique();

                    b.ToTable("UserRoleClaims", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.RefreshToken", b =>
                {
                    b.HasOne("Domain.Entities.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.RoleOperationClaim", b =>
                {
                    b.HasOne("Domain.Entities.OperationClaim", "OperationClaim")
                        .WithOne()
                        .HasForeignKey("Domain.Entities.RoleOperationClaim", "OperationClaimId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Role", "Role")
                        .WithOne()
                        .HasForeignKey("Domain.Entities.RoleOperationClaim", "RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OperationClaim");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Domain.Entities.Stream", b =>
                {
                    b.HasOne("Domain.Entities.Streamer", "Streamer")
                        .WithOne()
                        .HasForeignKey("Domain.Entities.Stream", "StreamerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Streamer");
                });

            modelBuilder.Entity("Domain.Entities.StreamBlockedUser", b =>
                {
                    b.HasOne("Domain.Entities.Streamer", "Streamer")
                        .WithMany()
                        .HasForeignKey("StreamerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Streamer");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.StreamChatMessage", b =>
                {
                    b.HasOne("Domain.Entities.Stream", "Stream")
                        .WithOne()
                        .HasForeignKey("Domain.Entities.StreamChatMessage", "StreamerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Streamer", "Streamer")
                        .WithOne()
                        .HasForeignKey("Domain.Entities.StreamChatMessage", "StreamerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User", "User")
                        .WithOne()
                        .HasForeignKey("Domain.Entities.StreamChatMessage", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Stream");

                    b.Navigation("Streamer");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.StreamFollowerUser", b =>
                {
                    b.HasOne("Domain.Entities.Streamer", "Streamer")
                        .WithMany()
                        .HasForeignKey("StreamerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Streamer");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.Streamer", b =>
                {
                    b.HasOne("Domain.Entities.User", "User")
                        .WithOne()
                        .HasForeignKey("Domain.Entities.Streamer", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.UserOperationClaim", b =>
                {
                    b.HasOne("Domain.Entities.OperationClaim", "OperationClaim")
                        .WithOne()
                        .HasForeignKey("Domain.Entities.UserOperationClaim", "OperationClaimId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User", "User")
                        .WithMany("UserOperationClaims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OperationClaim");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.UserRoleClaim", b =>
                {
                    b.HasOne("Domain.Entities.Role", "Role")
                        .WithOne()
                        .HasForeignKey("Domain.Entities.UserRoleClaim", "RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User", "User")
                        .WithMany("UserRoleClaims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Navigation("RefreshTokens");

                    b.Navigation("UserOperationClaims");

                    b.Navigation("UserRoleClaims");
                });
#pragma warning restore 612, 618
        }
    }
}
