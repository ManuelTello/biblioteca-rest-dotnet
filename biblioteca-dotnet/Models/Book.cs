namespace biblioteca_dotnet.Models
{
    public class Book
    {
        public int BookId { get; set; }

        public string? Title { get; set; }

        public string? DateWritten { get; set; }

        public Publisher Publisher { get; set; }

        public int PublisherId { get; set; }

        public List<Author> Authors { get; set; }

        public List<Genre> Genres { get; set; }
    }
}
