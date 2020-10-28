using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Bardcore.Models
{
    public partial class BardcoreContext : DbContext
    {
        public BardcoreContext()
        {
        }

        public BardcoreContext(DbContextOptions<BardcoreContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Genre> Genre { get; set; }
        public virtual DbSet<SongInfo> SongInfo { get; set; }
        public virtual DbSet<UserPlaylist> UserPlaylist { get; set; }
        public virtual DbSet<UserPlaylistTrack> UserPlaylistTrack { get; set; }
        public virtual DbSet<UserProfile> UserProfile { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\sqlexpress;Database=Bardcore;Trusted_Connection=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genre>(entity =>
            {
                entity.Property(e => e.GenreId).HasColumnName("GenreID");

                entity.Property(e => e.Gname)
                    .IsRequired()
                    .HasColumnName("GName")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SongInfo>(entity =>
            {
                entity.HasKey(e => e.TrackId);

                entity.Property(e => e.TrackId).HasColumnName("TrackID");

                entity.Property(e => e.Artist)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FileLocation).IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.UploadDate).HasColumnType("datetime");

                entity.HasOne(d => d.GenreNavigation)
                    .WithMany(p => p.SongInfo)
                    .HasForeignKey(d => d.Genre)
                    .HasConstraintName("FK__SongInfo__Genre__5441852A");
            });

            modelBuilder.Entity<UserPlaylist>(entity =>
            {
                entity.HasKey(e => e.PlaylistId);

                entity.Property(e => e.PlaylistId).HasColumnName("PlaylistID");

                entity.Property(e => e.PlaylistName)
                    .IsRequired()
                    .HasMaxLength(140)
                    .IsUnicode(false);

                entity.HasOne(d => d.PlaylistCreatorNavigation)
                    .WithMany(p => p.UserPlaylist)
                    .HasForeignKey(d => d.PlaylistCreator)
                    .HasConstraintName("FK__UserPlayl__Playl__412EB0B6");
            });

            modelBuilder.Entity<UserPlaylistTrack>(entity =>
            {
                entity.HasKey(e => e.PlaylistTrackId);

                entity.Property(e => e.PlaylistTrackId).HasColumnName("PlaylistTrackID");

                entity.Property(e => e.PlaylistId).HasColumnName("PlaylistID");

                entity.Property(e => e.TrackId).HasColumnName("TrackID");

                entity.HasOne(d => d.Playlist)
                    .WithMany(p => p.UserPlaylistTrack)
                    .HasForeignKey(d => d.PlaylistId)
                    .HasConstraintName("FK__UserPlayl__Playl__571DF1D5");

                entity.HasOne(d => d.Track)
                    .WithMany(p => p.UserPlaylistTrack)
                    .HasForeignKey(d => d.TrackId)
                    .HasConstraintName("FK__UserPlayl__Track__5812160E");
            });

            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.HasKey(e => e.Userid);

                entity.Property(e => e.Bio)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhotoPath)
                    .HasColumnName("photoPath")
                    .IsUnicode(false);

                entity.Property(e => e.UserAccountId)
                    .HasColumnName("user_account_id")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}
