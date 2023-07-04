namespace biblioteca_dotnet.Models
{
    public class Genre
    {
        public int GenreId { get; set; }

        public string? GenreName { get; set; }

        public List<Book> Books { get; set; }
    }
}
