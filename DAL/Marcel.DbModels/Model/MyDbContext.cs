using Microsoft.EntityFrameworkCore;

namespace Marcel.DbModels.Model
{
    public class MyDbContext : DbContext
    {
        public MyDbContext()
        {
        }

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=app.db");
            }
        }

        public virtual DbSet<Dish> Dish { get; set; }
    }
}