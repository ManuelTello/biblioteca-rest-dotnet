namespace biblioteca_dotnet.Helper
{
    public class Response<T>
    {
        public List<T>? Data { get; set; }

        public  int? StatusCode { get; set; }

        public string? InfoMessage { get; set; }
    }
}
