using System.Text.RegularExpressions;

namespace biblioteca_dotnet.Lib
{
    public class QueryFilter
    {
        public static string CleanString(string query)
        {
            Regex non_alpha_numeric = new Regex("[^a-zA-Z0-9#\\s]", RegexOptions.IgnoreCase);
            string query_trimmed = non_alpha_numeric.Replace(query, "");
            return query_trimmed;
        }

        public static List<string> CleanAndStripAlphaNumeric(string query)
        {
            string query_trimmed = QueryFilter.CleanString(query);
            List<string> query_filtered = query_trimmed.Split("#").ToList();
            return query_filtered;
        }


    }
}
