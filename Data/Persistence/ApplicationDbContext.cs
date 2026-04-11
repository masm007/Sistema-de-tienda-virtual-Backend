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

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserEntity>((ent) => {
                ent.ToTable("Users");
                ent.HasKey(e => e.Id);
                ent.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
                ent.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                ent.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                ent.Property(e => e.Email).IsRequired().HasMaxLength(100);
                ent.Property(e => e.Password).IsRequired();
                ent.Property(e => e.Role).IsRequired().HasDefaultValue(UserRole.User);
                ent.Ignore(e => e.Fullname);
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
