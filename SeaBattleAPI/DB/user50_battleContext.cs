using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SeaBattleAPI.Models;

namespace SeaBattleAPI.DB
{
    public partial class user50_battleContext : DbContext
    {
        public user50_battleContext()
        {
        }

        public user50_battleContext(DbContextOptions<user50_battleContext> options)
            : base(options)
        {
        }

        public virtual DbSet<EndGame> EndGames { get; set; } = null!;
        public virtual DbSet<Room> Rooms { get; set; } = null!;
        public virtual DbSet<Status> Statuses { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=192.168.200.35;database=user50_battle;user=user50;password=26643;TrustServerCertificate=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Cyrillic_General_100_CI_AI_KS_SC_UTF8");

            modelBuilder.Entity<EndGame>(entity =>
            {
                entity.ToTable("EndGame");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.GameTimeEnd).HasColumnType("datetime");

                entity.Property(e => e.RoomId).HasColumnName("Room_ID");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.EndGames)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EndGame_Room");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("Room");

                entity.Property(e => e.GameTimeStart).HasColumnType("datetime");

                entity.Property(e => e.StatusId).HasColumnName("Status_ID");

                entity.Property(e => e.UserCreatorId).HasColumnName("UserCreator_ID");

                entity.Property(e => e.UserSlowId).HasColumnName("UserSlow_ID");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Room_Status");

                entity.HasOne(d => d.UserCreator)
                    .WithMany(p => p.RoomUserCreators)
                    .HasForeignKey(d => d.UserCreatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Room_User");

                entity.HasOne(d => d.UserSlow)
                    .WithMany(p => p.RoomUserSlows)
                    .HasForeignKey(d => d.UserSlowId)
                    .HasConstraintName("FK_Room_User1");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Login).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
