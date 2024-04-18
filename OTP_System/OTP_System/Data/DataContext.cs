using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OTP_System.Models;

namespace OTP_System.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base ( options )
        {
        }

        public override DbSet<User> Users { get; set; }

    }
}
