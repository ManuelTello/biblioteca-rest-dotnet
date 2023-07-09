namespace biblioteca_dotnet.Helper
{
    public class Response<T>
    {
        public List<T>? Data { get; set; }

        public  int? StatusCode { get; set; }

        public string? InfoMessage { get; set; }

        public string? PreviousPage { get; set; }

        public string? NextPage { get; set; }

        public long? Page { get; set; }

        public long? MaxAmountOfPages { get; set; }
    }
}
