using JwtAuthentication.Model;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthentication.Data
{
    public class UserContext(DbContextOptions<UserContext> options): DbContext(options)
    {
        public DbSet<User> Users { get; set; } 
    }
}
