using biblioteca_dotnet.Data;
using biblioteca_dotnet.Models;
using Microsoft.EntityFrameworkCore;

namespace biblioteca_dotnet.Repositories
{
    public class BookRepository
    {
        private DataContext Context;
    
        public BookRepository(DataContext context)
        {
            this.Context = context;
        }

        public async Task<Book> FetchOneBookById(int id)
        {
            Book? book_fetched = await this.Context.Books
                .Where(b => b.BookId == id)
                .Include(b => b.Authors)
                .Include(b => b.Genres)
                .Include(b => b.Publisher)
                .SingleOrDefaultAsync();

            return book_fetched;
        }

        
        public async Task<List<Book>> FetchBooksByFilter(string title)
        {
            var a = this.Context.Books.FromSqlRaw("select * from dbo.Books")
                .Include(b => b.Authors)
                .Include(b => b.Publisher)
                .Include(b => b.Genres);
            foreach(var b in a)
            {
                var afsdaf = b;
            }
            List<Book> books_fetched = await this.Context.Books
                .Where(b => EF.Functions.Like(b.Title, title))
                .Include(b => b.Authors)
                .Include(b => b.Genres)
                .Include(b => b.Publisher)
                .ToListAsync();
            return books_fetched;
        }
    }
}
