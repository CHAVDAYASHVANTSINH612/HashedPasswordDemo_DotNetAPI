using Microsoft.EntityFrameworkCore;
using PasswordHashedDemo_DotNetAPI.Models;

namespace PasswordHashedDemo_DotNetAPI
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
