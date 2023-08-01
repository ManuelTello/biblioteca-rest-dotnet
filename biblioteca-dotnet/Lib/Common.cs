using biblioteca_dotnet.Models;
using biblioteca_dotnet.Helper;

namespace biblioteca_dotnet.Lib
{
    public class Common
    {
        public static bool ContainsAuthors(List<Author> authors, List<string> compareto)
        {

            foreach(string author_compare in compareto)
            {
                bool check_if_exists = authors.Any(a => a.AuthorName == author_compare);
                if(!check_if_exists)
                {
                    return false;
                }
            }
            return true;
        }       

        public static bool ContainsGenres(List<Genre> genres, List<string> compareto)
        {
            foreach (string genre_compare in compareto)
            {
                bool check_if_exists = genres.Any(a => a.GenreName == genre_compare);
                if (!check_if_exists)
                {
                   return false;
                }
            }
            return true;
        }

        public static bool ContainsThis(List<string> listA, List<string> listB)
        {
            foreach(string a in listA)
            {
                foreach(string b in listB)
                {
                    bool check_if_exists = a.Contains(b);
                    if(!check_if_exists)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static DateTime SerializeToDate(List<int>tvalues)
        {
            DateTime date_serialized = new DateTime(tvalues[0], tvalues[1], tvalues[2]);

            return date_serialized;
        }
    }
}
