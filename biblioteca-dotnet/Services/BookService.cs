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

        public BookService(DataContext context)
        {
            this.Repository = new BookRepository(context);
        }

        public async Task<Response<BookDTO>> FetchBookById(int id)
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
    
        public async Task<Response<BookDTO>> FetchByFilter(string title)
        {
            List<Book> books_fetched = await this.Repository.FetchBooksByFilter(title);
            Response<BookDTO> response = new Response<BookDTO>()
            {
                Data = Mapper.LEntityToDto(books_fetched),
                StatusCode = 200
            };
            return response;
        }
    }
}
