global using Microsoft.EntityFrameworkCore;
using MinimalCRUDWebAPI.Entities;

namespace MinimalCRUDWebAPI.Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<UserEntity> Users => Set<UserEntity>();
    }
}
