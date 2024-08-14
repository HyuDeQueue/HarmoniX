using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace HarmoniX_Repository.Models;

public partial class HarmonixDbContext : DbContext
{
    public HarmonixDbContext()
    {
    }

    public HarmonixDbContext(DbContextOptions<HarmonixDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Playlist> Playlists { get; set; }

    public virtual DbSet<Playlistssong> Playlistssongs { get; set; }

    public virtual DbSet<Song> Songs { get; set; }

    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        var strConn = config.GetConnectionString("DefaultConnectionStringDB");
        return strConn;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql(GetConnectionString(), Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.37-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PRIMARY");

            entity.ToTable("accounts");

            entity.HasIndex(e => e.Username, "Username").IsUnique();

            entity.Property(e => e.DisplayName).HasMaxLength(100);
            entity.Property(e => e.IsAuthorized).HasDefaultValueSql("'0'");
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.Username).HasMaxLength(100);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PRIMARY");

            entity.ToTable("categories");

            entity.HasIndex(e => e.CategoryName, "CategoryName").IsUnique();

            entity.Property(e => e.CategoryName).HasMaxLength(100);
        });

        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.HasKey(e => e.PlaylistId).HasName("PRIMARY");

            entity.ToTable("playlists");

            entity.HasIndex(e => e.AccountId, "AccountId");

            entity.Property(e => e.PlaylistDescription).HasMaxLength(1000);
            entity.Property(e => e.PlaylistName).HasMaxLength(100);

            entity.HasOne(d => d.Account).WithMany(p => p.Playlists)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("playlists_ibfk_1");
        });

        modelBuilder.Entity<Playlistssong>(entity =>
        {
            entity.HasKey(e => new { e.SongId, e.PlaylistId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("playlistssong");

            entity.HasIndex(e => e.PlaylistId, "PlaylistId");

            entity.HasOne(d => d.Playlist).WithMany(p => p.Playlistssongs)
                .HasForeignKey(d => d.PlaylistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("playlistssong_ibfk_2");

            entity.HasOne(d => d.Song).WithMany(p => p.Playlistssongs)
                .HasForeignKey(d => d.SongId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("playlistssong_ibfk_1");
        });

        modelBuilder.Entity<Song>(entity =>
        {
            entity.HasKey(e => e.SongId).HasName("PRIMARY");

            entity.ToTable("songs");

            entity.HasIndex(e => e.AccountId, "AccountId");

            entity.HasIndex(e => e.CategoryId, "CategoryId");

            entity.Property(e => e.ArtistName).HasMaxLength(100);
            entity.Property(e => e.SongMedia).HasColumnType("mediumtext");
            entity.Property(e => e.SongTitle).HasMaxLength(100);

            entity.HasOne(d => d.Account).WithMany(p => p.Songs)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("songs_ibfk_2");

            entity.HasOne(d => d.Category).WithMany(p => p.Songs)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("songs_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
