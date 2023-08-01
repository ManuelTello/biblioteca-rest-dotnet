namespace biblioteca_dotnet.Helper
{
    public class QueryModel
    {
        public string Title { get; set; } = string.Empty;

        public List<string>? Authors { get; set; }

        public List<string>? Genres { get; set; }

        public string? Publisher { get; set; }

        public Rented? Rented { get; set; }

        public Datepublished? DatePublished { get; set; }

        public int Page { get; set; }

        public int Take { get; set; }
    }
}
