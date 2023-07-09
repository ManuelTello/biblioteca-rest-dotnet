using biblioteca_dotnet.Data;
using biblioteca_dotnet.Dto;
using biblioteca_dotnet.Helper;
using biblioteca_dotnet.Models;
using biblioteca_dotnet.Repositories;
using biblioteca_dotnet.Lib;
using System.Linq;
using Microsoft.AspNetCore.Mvc.TagHelpers;

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

        public async Task<Response<BookDTO>> FetchByFilter(
            string title, int page, int take, string? authors_query, string? genres_query, string? publisher)
        {
            try
            {
                List<Book> books_fetched = await this.Repository.FetchBooksByFilter(title, page - 1, take);

                // Capaz cambiarlo por una lista KEY-VALUE es mejor
                if( authors_query != null || genres_query != null || publisher != null)
                {
                    List<Book> books_filtered = books_fetched.ToList();

                    foreach (Book book in books_fetched)
                    {

                        if (authors_query != null)
                        {
                            List<string> authors_query_cleaned = QueryFilter.CleanAndStripAlphaNumeric(authors_query);
                            List<bool> applies = new List<bool>();
                            foreach (string author in authors_query_cleaned)
                            {
                                applies.Add(book.Authors.Any(a => a.AuthorName == author));
                            }
                            if (books_filtered.Contains(book) && applies.Contains(false))
                            {
                                books_filtered.Remove(book);
                            }
                        }
                        if (genres_query != null)
                        {
                            List<string> genres_query_cleaned = QueryFilter.CleanAndStripAlphaNumeric(genres_query);
                            List<bool> applies = new List<bool>();
                            foreach (string genre in genres_query_cleaned)
                            {
                                applies.Add(book.Genres.Any(a => a.GenreName == genre));
                            }
                            if (books_filtered.Contains(book) && applies.Contains(false))
                            {
                                books_filtered.Remove(book);
                            }
                        }
                        if (publisher != null)
                        {
                            string publisher_query_cleaned = QueryFilter.CleanString(publisher);

                            if (book.Publisher.PublisherName != publisher_query_cleaned && books_filtered.Contains(book))
                            {
                                books_filtered.Remove(book);
                            }
                        }
                    }
                    books_fetched = books_filtered;
                }
                
                long amount_of_books = await this.Repository.FetchAmountOfFiltered(title);
                long max_amount_of_books = 1;

                while (max_amount_of_books * take <= amount_of_books)
                {
                    max_amount_of_books++;
                }

                Response<BookDTO> response = new Response<BookDTO>()
                {
                    Data = Mapper.LEntityToDto(books_fetched),
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
