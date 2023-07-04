using biblioteca_dotnet.Dto;
using biblioteca_dotnet.Models;

namespace biblioteca_dotnet.Lib
{
    public class Mapper
    {

        public static BookDTO UEntityToDto(Book book_entity)
        {
            List<string> authors = new List<string>();
            List<string> genres = new List<string>();

            foreach(Author author in book_entity.Authors)
            {
                authors.Add(author.AuthorName);
            }

            foreach(Genre genre in book_entity.Genres)
            {
                genres.Add(genre.GenreName);
            }

            BookDTO book_dto = new BookDTO()
            {
                Title = book_entity.Title,
                DateWritten = book_entity.DateWritten,
                Publisher = book_entity.Publisher.PublisherName,
                Authors = authors,
                Genres = genres
            };

            return book_dto;
        }

        public static List<BookDTO> LEntityToDto(List<Book> book_entity_list)
        {
            List<BookDTO> book_dto_list = new List<BookDTO>();
            foreach(Book book in book_entity_list)
            {
                book_dto_list.Add(UEntityToDto(book));
            }

            return book_dto_list;
        }

        public static Book UDtoToEntity(BookDTO book_dto)
        {
            List<Author> authors = new List<Author>();
            List<Genre> genres = new List<Genre>();

            foreach(string author in book_dto.Authors)
            {
                Author author_aux = new Author()
                {
                    AuthorName = author
                };
                authors.Add(author_aux);
            }

            foreach(string genre in book_dto.Genres)
            {
                Genre genre_aux = new Genre()
                {
                    GenreName = genre
                };
                genres.Add(genre_aux);
            }



            Book book_entity = new Book()
            {
                Title = book_dto.Title,
                DateWritten = book_dto.DateWritten,
                Publisher = new Publisher() { PublisherName = book_dto.Publisher},
                Authors = authors,
                Genres = genres
            };

            return book_entity;
        }
    }
}
