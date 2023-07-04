using Microsoft.EntityFrameworkCore;
using biblioteca_dotnet.Models;

namespace biblioteca_dotnet.Data
{
    public class DataContext:DbContext
    {
        public DbSet<Book> Books { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Publisher> Publishers { get; set; }

        private string? ConnectionString;

        public DataContext(IConfiguration config)
        {
            this.ConnectionString = config.GetConnectionString("sqlconnection");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.ConnectionString);
        }
    }
}
