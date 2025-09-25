using Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace DBContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        // Tablas
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de la relación 1:N (User -> Accounts)
            modelBuilder.Entity<Account>()
                .HasOne(a => a.User)       // cada cuenta tiene un usuario
                .WithMany(u => u.Accounts) // cada usuario tiene muchas cuentas
                .HasForeignKey(a => a.UserId); // FK en Account
        }
    }
}
