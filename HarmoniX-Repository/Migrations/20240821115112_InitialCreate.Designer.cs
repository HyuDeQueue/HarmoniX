﻿// <auto-generated />
using System;
using HarmoniX_Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HarmoniX_Repository.Migrations
{
    [DbContext(typeof(HarmonixDbContext))]
    [Migration("20240821115112_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.HasCharSet(modelBuilder, "utf8mb4");
            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("HarmoniX_Repository.Models.Account", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("AccountId"));

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int?>("IsAuthorized")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValueSql("'0'");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("AccountId")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "Username" }, "Username")
                        .IsUnique();

                    b.ToTable("accounts", (string)null);
                });

            modelBuilder.Entity("HarmoniX_Repository.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("CategoryId")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "CategoryName" }, "CategoryName")
                        .IsUnique();

                    b.ToTable("categories", (string)null);
                });

            modelBuilder.Entity("HarmoniX_Repository.Models.Playlist", b =>
                {
                    b.Property<int>("PlaylistId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("PlaylistId"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<string>("PlaylistDescription")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("varchar(1000)");

                    b.Property<string>("PlaylistName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("PlaylistId")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "AccountId" }, "AccountId");

                    b.ToTable("playlists", (string)null);
                });

            modelBuilder.Entity("HarmoniX_Repository.Models.Playlistssong", b =>
                {
                    b.Property<int>("SongId")
                        .HasColumnType("int");

                    b.Property<int>("PlaylistId")
                        .HasColumnType("int");

                    b.Property<int>("Position")
                        .HasColumnType("int");

                    b.HasKey("SongId", "PlaylistId")
                        .HasName("PRIMARY")
                        .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                    b.HasIndex(new[] { "PlaylistId" }, "PlaylistId");

                    b.ToTable("playlistssong", (string)null);
                });

            modelBuilder.Entity("HarmoniX_Repository.Models.Song", b =>
                {
                    b.Property<int>("SongId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("SongId"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<string>("ArtistName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("SongMedia")
                        .IsRequired()
                        .HasColumnType("mediumtext");

                    b.Property<string>("SongTitle")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("SongId")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "AccountId" }, "AccountId")
                        .HasDatabaseName("AccountId1");

                    b.HasIndex(new[] { "CategoryId" }, "CategoryId");

                    b.ToTable("songs", (string)null);
                });

            modelBuilder.Entity("HarmoniX_Repository.Models.Playlist", b =>
                {
                    b.HasOne("HarmoniX_Repository.Models.Account", "Account")
                        .WithMany("Playlists")
                        .HasForeignKey("AccountId")
                        .IsRequired()
                        .HasConstraintName("playlists_ibfk_1");

                    b.Navigation("Account");
                });

            modelBuilder.Entity("HarmoniX_Repository.Models.Playlistssong", b =>
                {
                    b.HasOne("HarmoniX_Repository.Models.Playlist", "Playlist")
                        .WithMany("Playlistssongs")
                        .HasForeignKey("PlaylistId")
                        .IsRequired()
                        .HasConstraintName("playlistssong_ibfk_2");

                    b.HasOne("HarmoniX_Repository.Models.Song", "Song")
                        .WithMany("Playlistssongs")
                        .HasForeignKey("SongId")
                        .IsRequired()
                        .HasConstraintName("playlistssong_ibfk_1");

                    b.Navigation("Playlist");

                    b.Navigation("Song");
                });

            modelBuilder.Entity("HarmoniX_Repository.Models.Song", b =>
                {
                    b.HasOne("HarmoniX_Repository.Models.Account", "Account")
                        .WithMany("Songs")
                        .HasForeignKey("AccountId")
                        .IsRequired()
                        .HasConstraintName("songs_ibfk_2");

                    b.HasOne("HarmoniX_Repository.Models.Category", "Category")
                        .WithMany("Songs")
                        .HasForeignKey("CategoryId")
                        .IsRequired()
                        .HasConstraintName("songs_ibfk_1");

                    b.Navigation("Account");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("HarmoniX_Repository.Models.Account", b =>
                {
                    b.Navigation("Playlists");

                    b.Navigation("Songs");
                });

            modelBuilder.Entity("HarmoniX_Repository.Models.Category", b =>
                {
                    b.Navigation("Songs");
                });

            modelBuilder.Entity("HarmoniX_Repository.Models.Playlist", b =>
                {
                    b.Navigation("Playlistssongs");
                });

            modelBuilder.Entity("HarmoniX_Repository.Models.Song", b =>
                {
                    b.Navigation("Playlistssongs");
                });
#pragma warning restore 612, 618
        }
    }
}
