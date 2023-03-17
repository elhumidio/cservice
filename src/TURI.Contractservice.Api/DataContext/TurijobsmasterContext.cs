using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TURI.Contractservice.DataContext
{
    public partial class TurijobsmasterContext : DbContext
    {
        public TurijobsmasterContext()
        {
        }

        public TurijobsmasterContext(DbContextOptions<TurijobsmasterContext> options)
            : base(options)
        {
        }

        public virtual DbSet<OigSafety> OigSafeties { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=mssql-brandturijobs.live.stepstone.tools;user=tjweb;password=g32qmYUId;database=Turijobs.master");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Modern_Spanish_CI_AS");

            modelBuilder.Entity<OigSafety>(entity =>
            {
                entity.ToTable("OIG_safety");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Column0)
                    .HasColumnType("text")
                    .HasColumnName("Column 0");

                entity.Property(e => e.Column1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Column 1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
