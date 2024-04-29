using Models.Game;
using Microsoft.EntityFrameworkCore;

namespace RentGames.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Game> Games { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("DataSource=rentgames.sqlite;Cache=Shared");
            
    }
}