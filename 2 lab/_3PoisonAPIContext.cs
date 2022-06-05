using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DB_lab2
{
    public partial class _3PoisonAPIContext : DbContext
    {
        public _3PoisonAPIContext()
        {
        }

        public _3PoisonAPIContext(DbContextOptions<_3PoisonAPIContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; } = null!;
        public virtual DbSet<HP> HPs { get; set; } = null!;
        public virtual DbSet<Herbalist> Herbalists { get; set; } = null!;
        public virtual DbSet<PP> PPs { get; set; } = null!;
        public virtual DbSet<Poison> Poisons { get; set; } = null!;
        public virtual DbSet<Poisoner> Poisoners { get; set; } = null!;
        public virtual DbSet<Herb_P> Herbs_Ps { get; set; } = null!;
        public virtual DbSet<Herb> Herbs { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server = WIN-EPG4JRCB2OV; Database = 3PoisonAPI; Trusted_Connection= True; ");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HP>(entity =>
            {
                entity.ToTable("H_Ps");

                entity.HasIndex(e => e.HerbalistId, "IX_H_Ps_HerbalistId");

                entity.HasIndex(e => e.PoisonerId, "IX_H_Ps_PoisonerId");

                entity.HasOne(d => d.Herbalist)
                    .WithMany(p => p.HPs)
                    .HasForeignKey(d => d.HerbalistId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Poisoner)
                    .WithMany(p => p.HPs)
                    .HasForeignKey(d => d.PoisonerId);
            });

            modelBuilder.Entity<PP>(entity =>
            {
                entity.ToTable("P_Ps");

                entity.HasIndex(e => e.PoisonId, "IX_P_Ps_PoisonId");

                entity.HasIndex(e => e.PoisonerId, "IX_P_Ps_PoisonerId");

                entity.HasOne(d => d.Poison)
                    .WithMany(p => p.PPs)
                    .HasForeignKey(d => d.PoisonId);

                entity.HasOne(d => d.Poisoner)
                    .WithMany(p => p.PPs)
                    .HasForeignKey(d => d.PoisonerId);
            });

            modelBuilder.Entity<Herb_P>(entity =>
            {
                entity.ToTable("Herbs_Ps");

                entity.HasIndex(e => e.HerbId, "IX_Herbs_Ps_HerbId");

                entity.HasIndex(e => e.PoisonerId, "IX_Herbs_Ps_PoisonerId");

                entity.HasOne(d => d.Herb)
                    .WithMany(p => p.Herbs_Ps)
                    .HasForeignKey(d => d.HerbId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Poisoner)
                    .WithMany(p => p.Herbs_Ps)
                    .HasForeignKey(d => d.PoisonerId);
            });

            modelBuilder.Entity<Poisoner>(entity =>
            {
                entity.HasIndex(e => e.AddressId, "IX_Poisoners_AddressId");

                entity.Property(e => e.BirthDate).HasColumnType("int");

                entity.Property(e => e.Info).HasColumnName("info");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Poisoners)
                    .HasForeignKey(d => d.AddressId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
