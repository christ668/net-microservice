using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace common.Model
{
    public partial class DemoDbContext : DbContext
    {
        public DemoDbContext()
        {
        }

        public DemoDbContext(DbContextOptions<DemoDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Activeorder> Activeorders { get; set; } = null!;
        public virtual DbSet<Composition> Compositions { get; set; } = null!;
        public virtual DbSet<Guesttable> Guesttables { get; set; } = null!;
        public virtual DbSet<Recipe> Recipes { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Userdetail> Userdetails { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;port=3306;user=root;database=demo", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.25-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Activeorder>(entity =>
            {
                entity.HasKey(e => new { e.IdRecipe, e.IdGuestTable })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("activeorder");

                entity.HasIndex(e => e.IdGuestTable, "fk_table3_table2");

                entity.Property(e => e.IdRecipe)
                    .HasColumnType("int(11)")
                    .HasColumnName("idRecipe");

                entity.Property(e => e.IdGuestTable)
                    .HasColumnType("int(11)")
                    .HasColumnName("idGuestTable");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createDate");

                entity.HasOne(d => d.IdGuestTableNavigation)
                    .WithMany(p => p.Activeorders)
                    .HasForeignKey(d => d.IdGuestTable)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_table3_table2");

                entity.HasOne(d => d.IdRecipeNavigation)
                    .WithMany(p => p.Activeorders)
                    .HasForeignKey(d => d.IdRecipe)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_table3_table1");
            });

            modelBuilder.Entity<Composition>(entity =>
            {
                entity.ToTable("composition");

                entity.HasIndex(e => e.RecipeId, "recipeID");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Dose).HasColumnName("dose");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.RecipeId)
                    .HasColumnType("int(11)")
                    .HasColumnName("recipeID");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.Compositions)
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("composition_ibfk_1");
            });

            modelBuilder.Entity<Guesttable>(entity =>
            {
                entity.ToTable("guesttable");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Chair).HasColumnType("int(11)");

                entity.Property(e => e.PlacementDate).HasColumnType("datetime");

                entity.Property(e => e.RoomPosition).HasMaxLength(50);
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.ToTable("recipe");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Price).HasColumnName("price");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.EnrollmentDate).HasColumnType("datetime");

                entity.Property(e => e.FirstName).HasMaxLength(45);

                entity.Property(e => e.LastName).HasMaxLength(45);

                entity.Property(e => e.Password).HasMaxLength(200);

                entity.Property(e => e.Username).HasMaxLength(45);
            });

            modelBuilder.Entity<Userdetail>(entity =>
            {
                entity.ToTable("userdetail");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ActiveAddress).HasMaxLength(200);

                entity.Property(e => e.MaritalStatus).HasMaxLength(200);

                entity.Property(e => e.Nik)
                    .HasMaxLength(200)
                    .HasColumnName("NIK");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Userdetail)
                    .HasForeignKey<Userdetail>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("userdetail_ibfk_1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
