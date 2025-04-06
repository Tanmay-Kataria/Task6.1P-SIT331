using Microsoft.EntityFrameworkCore;
using robot_controller_api;

namespace robot_controller_api.Persistence
{
    /// <summary>
    /// The Entity Framework Core DbContext for PostgreSQL.
    /// </summary>
    public class UserDataAccess : DbContext
    {
        public UserDataAccess(DbContextOptions<UserDataAccess> options) : base(options) { }

        public DbSet<UserModel> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure UserModel
            modelBuilder.Entity<UserModel>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
                entity.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(u => u.LastName).IsRequired().HasMaxLength(100);
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.Description).HasMaxLength(500);
                entity.Property(u => u.Role).HasMaxLength(50);
                entity.Property(u => u.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(u => u.ModifiedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }
    }
}
