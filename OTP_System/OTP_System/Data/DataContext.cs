using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OTP_System.Models;

namespace OTP_System.Data
{
    public class DataContext : IdentityDbContext<User, IdentityRole, string>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public override DbSet<User> Users { get; set; }

        public DbSet<Otp> Otps { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); 

            builder.Entity<User>()
                .HasMany(u => u.Otps)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId)
                .IsRequired();
        }
    }
}
