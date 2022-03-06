global using Microsoft.EntityFrameworkCore;
using MinimalCRUDWebAPI.Models;

namespace MinimalCRUDWebAPI.Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<UserModel> Users => Set<UserModel>();
    }
}
