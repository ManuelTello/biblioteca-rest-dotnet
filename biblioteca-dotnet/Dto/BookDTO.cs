namespace biblioteca_dotnet.Dto
{
    public class BookDTO
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? DateWritten { get; set; }

        public string? Publisher { get; set; }

        public int Rented { get; set; }

        public List<string> Authors { get; set; }

        public List<string> Genres { get; set; }
    }
}
