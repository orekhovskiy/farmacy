using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Farmacy.Models.Context
{
    public partial class MedicineApiContext : DbContext
    {
        public MedicineApiContext()
        {
        }

        public MedicineApiContext(DbContextOptions<MedicineApiContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Component> Component { get; set; }
        public virtual DbSet<Form> Form { get; set; }
        public virtual DbSet<Medicine> Medicine { get; set; }
        public virtual DbSet<MedicineComposition> MedicineComposition { get; set; }
        public virtual DbSet<Position> Position { get; set; }
        public virtual DbSet<Producer> Producer { get; set; }
        public virtual DbSet<Purchase> Purchase { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=spbdevsql01\\dev;Database=farmacy_db;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UQ__Category__72E12F1BC3AFCA92")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<Component>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UQ__Componen__72E12F1B0A7FAA20")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<Form>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UQ__Form__72E12F1BAAC0D8C7")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<Medicine>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UQ__Medicine__72E12F1B6F62BF3D")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CategoryId).HasColumnName("categoryId");

                entity.Property(e => e.Count).HasColumnName("count");

                entity.Property(e => e.FormId).HasColumnName("formId");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(30);

                entity.Property(e => e.ProducerId).HasColumnName("producerId");

                entity.Property(e => e.ShelfTime).HasColumnName("shelfTime");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Medicine)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Medicine__catego__5006DFF2");

                entity.HasOne(d => d.Form)
                    .WithMany(p => p.Medicine)
                    .HasForeignKey(d => d.FormId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Medicine__formId__50FB042B");

                entity.HasOne(d => d.Producer)
                    .WithMany(p => p.Medicine)
                    .HasForeignKey(d => d.ProducerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Medicine__produc__4F12BBB9");
            });

            modelBuilder.Entity<MedicineComposition>(entity =>
            {
                entity.HasIndex(e => new { e.MedicineId, e.ComponentId })
                    .HasName("UQ__Medicine__CCF9C9C3F2BE87C3")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ComponentId).HasColumnName("componentId");

                entity.Property(e => e.MedicineId).HasColumnName("medicineId");

                entity.HasOne(d => d.Component)
                    .WithMany(p => p.MedicineComposition)
                    .HasForeignKey(d => d.ComponentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MedicineC__compo__56B3DD81");

                entity.HasOne(d => d.Medicine)
                    .WithMany(p => p.MedicineComposition)
                    .HasForeignKey(d => d.MedicineId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MedicineC__medic__55BFB948");
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UQ__Position__72E12F1BF362C253")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<Producer>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UQ__Producer__72E12F1B96AC8197")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.MedicineId).HasColumnName("medicineId");

                entity.Property(e => e.Operation).HasColumnName("operation");

                entity.Property(e => e.PurchaseDate)
                    .HasColumnName("purchaseDate")
                    .HasColumnType("date");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.Medicine)
                    .WithMany(p => p.Purchase)
                    .HasForeignKey(d => d.MedicineId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Purchase__medici__5A846E65");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Purchase)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Purchase__userId__59904A2C");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Login)
                    .HasName("UQ__User__7838F272AFCB1D7B")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasColumnName("firstname")
                    .HasMaxLength(30);

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasColumnName("lastname")
                    .HasMaxLength(30);

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasColumnName("login")
                    .HasMaxLength(15);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(32);

                entity.Property(e => e.Position).HasColumnName("position");

                entity.HasOne(d => d.PositionNavigation)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.Position)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__User__position__3BFFE745");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
