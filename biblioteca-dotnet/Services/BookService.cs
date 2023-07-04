using biblioteca_dotnet.Data;
using biblioteca_dotnet.Dto;
using biblioteca_dotnet.Helper;
using biblioteca_dotnet.Models;
using biblioteca_dotnet.Repositories;
using biblioteca_dotnet.Lib;

namespace biblioteca_dotnet.Services
{
    public class BookService
    {
        private readonly BookRepository Repository;
        private readonly string Enviorment;

        public BookService(DataContext context, string env)
        {
            this.Repository = new BookRepository(context);
            this.Enviorment = env;
        }

        public async Task<Response<BookDTO>> FetchById(int id)
        {
            try
            {
                Book? book_fetched = await this.Repository.FetchOneBookById(id);
                if (book_fetched != null)
                {
                    Response<BookDTO> response = new Response<BookDTO>()
                    {
                        Data = new List<BookDTO>() { Mapper.UEntityToDto(book_fetched) },
                        StatusCode = 200,
                    };
                    return response;
                }
                else
                {
                    Response<BookDTO> response = new Response<BookDTO>()
                    {
                        StatusCode = 404,
                        InfoMessage = "Book not found"
                    };
                    return response;
                }
            }catch(OperationCanceledException ex)
            {
                Response<BookDTO> response = new Response<BookDTO>()
                {
                    StatusCode = 500,
                    InfoMessage = "There was a problem processing your request"
                };
                return response;
            }
        }

        public async Task<Response<BookDTO>> FetchByFilter(string title, int skip, int totake)
        {
            try
            {
                List<Book> books_fetched = await this.Repository.FetchBooksByFilter(title, skip - 1, totake);
                long amount_of_books = await this.Repository.FetchAmountOfFiltered(title);
                Response<BookDTO> response = new Response<BookDTO>()
                {
                    Data = Mapper.LEntityToDto(books_fetched),
                    StatusCode = 200,
                    Page = skip,
                    PreviousPage = skip - 1 == 0 ? null : $"/api/Books/SearchByFilter?title={title}&skip={skip - 1}&totake={totake}",
                    NextPage = (skip * totake) >= amount_of_books ? null : $"/api/Books/SearchByFilter?title={title}&skip={skip + 1}&totake={totake}"
                };
                return response;
            }
            catch (OperationCanceledException ex)
            {
                return new Response<BookDTO>()
                {
                    StatusCode = 500,
                };
            }
        }

        public async Task<Response<BookDTO>> FetchMostRented()
        {
            try
            {
                List<Book> most_rented_books = await this.Repository.FetchTopMostRented();
                Response<BookDTO> response = new Response<BookDTO>()
                {
                    Data = Mapper.LEntityToDto(most_rented_books),
                    StatusCode = 200
                };
                return response;
            }
            catch (OperationCanceledException ex)
            {
                Response<BookDTO> response = new Response<BookDTO>()
                {
                    StatusCode = 500
                };
                return response;
            }
        }
    }
}
