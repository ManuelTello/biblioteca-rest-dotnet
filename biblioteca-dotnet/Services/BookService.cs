using biblioteca_dotnet.Data;
using biblioteca_dotnet.Dto;
using biblioteca_dotnet.Helper;
using biblioteca_dotnet.Models;
using biblioteca_dotnet.Repositories;
using biblioteca_dotnet.Lib;
using System.Linq;

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

        public async Task<Response<BookDTO>> FetchByFilter(string query, string title)
        {
            try
            {
                QueryModel query_model = QueryOperations.DeserializeQuery(query);
                int page = query_model.Page;
                int take = query_model.Take;
                List<Book> books_fetched = await this.Repository.FetchBooksByFilter(query_model.Title, page - 1, take);
                List<Book> books_filtered = books_fetched.ToList();
                
                if(query_model.Authors != null)
                {
                    foreach(Book book in books_fetched)
                    {
                        bool authors_exists = Common.ContainsAuthors(book.Authors, query_model.Authors);
                        if(!authors_exists)
                        {
                            if (books_filtered.Contains(book))
                            {
                                books_filtered.Remove(book);
                            }
                        }
                    }
                }
                
                if(query_model.Genres != null)
                {
                    foreach(Book book in books_fetched)
                    {
                        bool genres_exists = Common.ContainsGenres(book.Genres, query_model.Genres);
                        if (!genres_exists) 
                        { 
                            if (books_filtered.Contains(book))
                            {
                                books_filtered.Remove(book);
                            }
                        }
                    }
                }
                
                if(query_model.Publisher != null)
                {
                    foreach(Book book in books_fetched)
                    {
                        bool publisher_exist = book.Publisher.PublisherName == query_model.Publisher ? true : false;
                        if(!publisher_exist)
                        {
                            if(books_filtered.Contains(book))
                            {
                                books_filtered.Remove(book);
                            }
                        }
                    }
                }

                long amount_of_books = await this.Repository.FetchAmountOfFiltered(title);
                long max_amount_of_books = 1;

                while (max_amount_of_books * take <= amount_of_books)
                {
                    max_amount_of_books++;
                }

                Response<BookDTO> response = new Response<BookDTO>()
                {
                    Data = Mapper.LEntityToDto(books_filtered),
                    StatusCode = 200,
                    Page = page,
                    PreviousPage = page - 1 == 0 ? null : $"/api/Books/SearchByFilter?title={title}&skip={page - 1}&totake={take}",
                    NextPage = (page * take) >= amount_of_books ? null : $"/api/Books/SearchByFilter?title={title}&skip={page + 1}&totake={take}",
                    MaxAmountOfPages = max_amount_of_books         
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
