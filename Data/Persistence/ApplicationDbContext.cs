using Domain.Entity;
using Domain.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Persistence {
    public class ApplicationDbContext : DbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {

        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ProductImageEntity> ProductImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserEntity>((ent) => {
                ent.ToTable("Users");
                ent.HasKey(e => e.Id);
                ent.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
                ent.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                ent.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                ent.Property(e => e.Email).IsRequired().HasMaxLength(100);
                ent.HasIndex(e => e.Email).IsUnique();
                ent.Property(e => e.Password).IsRequired();
                ent.Property(e => e.Role).IsRequired().HasDefaultValue(UserRole.User);
                ent.Ignore(e => e.Fullname);
                ent.Property<DateTime>("CreatedAt").IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
                ent.Property<DateTime>("UpdatedAt").IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");
            });
            modelBuilder.Entity<RefreshTokenEntity>(ent => {
                ent.ToTable("RefreshTokens");
                ent.HasKey(e => e.Id);
                ent.Property(e => e.TokenHash).IsRequired();
                ent.HasIndex(e => e.TokenHash).IsUnique();
                ent.Property(e => e.Expiration).IsRequired();
                ent.Property(e => e.IsRevoked).IsRequired();
                ent.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                // muchos tokens tienen un usuario
                ent.HasOne(rt => rt.User).WithMany(u => u.RefreshTokens).HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<CategoryEntity>((ent) => {
                ent.ToTable("Categories");
                ent.HasKey(e => e.Id);
                ent.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
                ent.Property(e => e.Name).IsRequired().HasMaxLength(50);
                ent.HasIndex(e => e.Name).IsUnique();
                ent.Property(e => e.Description).IsRequired().HasMaxLength(100);
                ent.Property<DateTime>("CreatedAt").IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
                ent.Property<DateTime>("UpdatedAt").IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");
            });
            modelBuilder.Entity<ProductEntity>((ent) => {
                ent.ToTable("Products");
                ent.HasKey(e => e.Id);
                ent.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
                ent.Property(e => e.Name).IsRequired().HasMaxLength(50);
                ent.Property(e => e.Description).IsRequired().HasMaxLength(100);
                ent.Property(e => e.Sku).IsRequired().HasMaxLength(40);
                ent.HasIndex(e => e.Sku).IsUnique();
                ent.Property(e => e.Price).IsRequired().HasPrecision(10, 2);
                ent.Property(e => e.Quantity).IsRequired();
                ent.Property(e => e.IsAvailable).IsRequired().HasDefaultValue(true);
                ent.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
                //muchos productos tienen una categoria
                ent.HasOne(e => e.Category).WithMany(cat => cat.Products).HasForeignKey(e => e.CategoryId).OnDelete(DeleteBehavior.Restrict);
                //Images no es necesario de hacerlo de ambos lados
                ent.Property<DateTime>("CreatedAt").IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
                ent.Property<DateTime>("UpdatedAt").IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");
            });
            modelBuilder.Entity<ProductImageEntity>((ent) => {
                ent.ToTable("ProductImages");
                ent.HasKey(e => e.Id);
                ent.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
                ent.Property(e => e.CloudinaryPublicId).IsRequired().HasMaxLength(200);
                ent.Property(e => e.ImageUrl).IsRequired().HasMaxLength(1000);
                ent.HasIndex(e => e.ProductId);
                //muchos imagenes tienen un producto
                ent.HasOne(e => e.Product).WithMany(p => p.Images).HasForeignKey(e => e.ProductId).OnDelete(DeleteBehavior.Cascade);
                ent.Property<DateTime>("CreatedAt").IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
                ent.Property<DateTime>("UpdatedAt").IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");
            });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
            //extras
            UpdateTimeStamp();
            return base.SaveChangesAsync(cancellationToken);
        } 

        private void UpdateTimeStamp() {
            var entries = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified);
            foreach (var entry in entries) {
                if (entry.Metadata.FindProperty("UpdatedAt") != null) {
                    entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
                }
            }
        }
    }
}
