using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FarmacyWebApi.Models
{
    public partial class FarmacyWebApiContext : DbContext
    {
        public FarmacyWebApiContext()
        {
        }

        public FarmacyWebApiContext(DbContextOptions<FarmacyWebApiContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Change> Change { get; set; }
        public virtual DbSet<Component> Component { get; set; }
        public virtual DbSet<Form> Form { get; set; }
        public virtual DbSet<Medicine> Medicine { get; set; }
        public virtual DbSet<MedicineComposition> MedicineComposition { get; set; }
        public virtual DbSet<Position> Position { get; set; }
        public virtual DbSet<Producer> Producer { get; set; }
        public virtual DbSet<User> User { get; set; }

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=spbdevsql01\\dev;Database=farmacy_db;Trusted_Connection=True;");
            }
        }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UQ__Category__72E12F1BF307AB04")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Change>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ChangeDate)
                    .HasColumnName("changeDate")
                    .HasColumnType("date");

                entity.Property(e => e.MedicineId).HasColumnName("medicineId");

                entity.Property(e => e.Operation).HasColumnName("operation");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.Medicine)
                    .WithMany(p => p.Change)
                    .HasForeignKey(d => d.MedicineId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Change__medicine__0880433F");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Change)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Change__userId__078C1F06");
            });

            modelBuilder.Entity<Component>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UQ__Componen__72E12F1B508A7CB8")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Form>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UQ__Form__72E12F1B0D450EE8")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Medicine>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UQ__Medicine__72E12F1BA3771A93")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CategoryId).HasColumnName("categoryId");

                entity.Property(e => e.Count).HasColumnName("count");

                entity.Property(e => e.FormId).HasColumnName("formId");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.ProducerId).HasColumnName("producerId");

                entity.Property(e => e.ShelfTime).HasColumnName("shelfTime");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Medicine)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Medicine__catego__7E02B4CC");

                entity.HasOne(d => d.Form)
                    .WithMany(p => p.Medicine)
                    .HasForeignKey(d => d.FormId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Medicine__formId__7EF6D905");

                entity.HasOne(d => d.Producer)
                    .WithMany(p => p.Medicine)
                    .HasForeignKey(d => d.ProducerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Medicine__produc__7D0E9093");
            });

            modelBuilder.Entity<MedicineComposition>(entity =>
            {
                entity.HasIndex(e => new { e.MedicineId, e.ComponentId })
                    .HasName("UQ__Medicine__CCF9C9C3C795DBFA")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ComponentId).HasColumnName("componentId");

                entity.Property(e => e.MedicineId).HasColumnName("medicineId");

                entity.HasOne(d => d.Component)
                    .WithMany(p => p.MedicineComposition)
                    .HasForeignKey(d => d.ComponentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MedicineC__compo__04AFB25B");

                entity.HasOne(d => d.Medicine)
                    .WithMany(p => p.MedicineComposition)
                    .HasForeignKey(d => d.MedicineId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MedicineC__medic__03BB8E22");
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UQ__Position__72E12F1BBFFA7A0B")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Producer>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UQ__Producer__72E12F1B314E1880")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Login)
                    .HasName("UQ__User__7838F2721B942848")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasColumnName("firstname")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasColumnName("lastname")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasColumnName("login")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(32)
                    .IsFixedLength();

                entity.Property(e => e.Position).HasColumnName("position");

                entity.HasOne(d => d.PositionNavigation)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.Position)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__User__position__793DFFAF");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
