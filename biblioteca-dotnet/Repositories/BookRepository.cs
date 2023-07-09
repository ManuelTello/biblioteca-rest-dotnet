using biblioteca_dotnet.Data;
using biblioteca_dotnet.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.RegularExpressions;

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

        public async Task<List<Book>> FetchBooksByFilter(string title, int page, int take)
        {
            List < Book > books_fetched = await this.Context.Books
                .Where(b => EF.Functions.Like(b.Title, $"%{title}%"))
                .Include(b => b.Authors)
                .Include(b => b.Publisher)
                .Include(b => b.Genres)
                .Skip(page)
                .Take(take)
                .ToListAsync();
            return books_fetched;
        }

        public async Task<long> FetchAmountOfFiltered(string title)
        {
            long amount_of_books = await this.Context.Books
                .Where(b => EF.Functions.Like(b.Title, $"%{title}%"))
                .CountAsync();
            return amount_of_books;
        }

        public async Task<List<Book>> FetchTopMostRented()
        {
            List<Book> most_rented_books = await this.Context.Books
                .Include(b => b.Authors)
                .Include(b => b.Genres)
                .Include(b => b.Publisher)
                .OrderByDescending(b => b.Rented)
                .Take(10)
                .ToListAsync();
            return most_rented_books;
        }
    }
}
