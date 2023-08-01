using biblioteca_dotnet.Helper;

namespace biblioteca_dotnet.Lib
{
    public class QueryOperations
    {
        public const string QueryFieldSeparator = "$";

        public const string QueryMetadataSeparator = ":";

        public const string QueryEquals = "=";

        public const string QueryValuesSeparator = "%";

        public const string QueryWordSeparator = "_";

        public const string QueryDateYMDSeparator = "-";

        public static string? ExtractSingleValueFromQuery(string query, string to_extract)
        {
            List<string> query_stripped = query.Split(QueryFieldSeparator).ToList();

            query_stripped.Remove(query_stripped.First());

            string? query_extraction = null;

            foreach (string field in query_stripped)
            {
                List<string> tagvalue = field.Split(QueryEquals).ToList();

                string tag = tagvalue[0];
                string value = tagvalue[1];

                if (tag.ToLower() == to_extract.ToLower())
                {
                    if (value.Contains(QueryMetadataSeparator))
                    {
                        query_extraction = value.Substring(0, value.IndexOf(QueryMetadataSeparator));
                    }
                    else
                    {
                        query_extraction = value;
                    }
                }
            }
            return query_extraction;
        }

        public static List<string>? ExtractListValuesFromQuery(string query, string to_extract)
        {
            List<string> query_stripped = query.Split(QueryFieldSeparator).ToList();

            query_stripped.Remove(query_stripped.First());

            List<string>? query_extraction = null;

            foreach (string field in query_stripped)
            {
                List<string> keyvalue = field.Split(QueryEquals).ToList();

                string tag = keyvalue[0];
                string value = keyvalue[1];

                if (tag.ToLower() == to_extract.ToLower())
                {
                    string value_formated = value.Replace(QueryWordSeparator, " ");
                    query_extraction = value_formated.Split(QueryValuesSeparator).ToList();
                }
            }
            return query_extraction;
        }

        public static string? ExtactMetadataFromQueryField(string query, string to_extract)
        {
            List<string> query_stripped = query.Split(QueryFieldSeparator).ToList();

            query_stripped.Remove(query_stripped.First());

            string? query_metadata = null;

            foreach (string field in query_stripped)
            {
                List<string> keyvalue = field.Split(QueryEquals).ToList();

                string tag = keyvalue[0];
                string value = keyvalue[1];

                if (tag.ToLower() == to_extract.ToLower())
                {
                    int separator_position = value.IndexOf(QueryMetadataSeparator);
                    query_metadata = value.Substring(separator_position + 1);
                }
            }
            return query_metadata;
        }

        public static bool IfExists(string query, string tag)
        {
            return query.Contains(tag);
        }

        public static DateTime CleanAndParseTimestampQuery(string query)
        {
            string timestamp_string = QueryOperations.ExtractSingleValueFromQuery(query, "date");

            List<int> timestamp_values = new List<int>();

            foreach(string element  in timestamp_string.Split(QueryDateYMDSeparator))
            {
                timestamp_values.Add(Convert.ToInt32(element));
            }
            return Common.SerializeToDate(timestamp_values);
        }
    
        public static QueryModel DeserializeQuery(string query)
        {
            QueryModel query_model = new QueryModel()
            {
                Title = QueryOperations.ExtractSingleValueFromQuery(query, "title"),
                Publisher = QueryOperations.ExtractSingleValueFromQuery(query, "publisher"),
                Page = Convert.ToInt32(QueryOperations.ExtractSingleValueFromQuery(query, "page")),
                Take = Convert.ToInt32(QueryOperations.ExtractSingleValueFromQuery(query, "take")),
                Authors = QueryOperations.ExtractListValuesFromQuery(query, "authors"),
                Genres = QueryOperations.ExtractListValuesFromQuery(query, "genres"),
                Rented = QueryOperations.IfExists(query, "rented") ? new Rented()
                {
                    Value = Convert.ToInt32(QueryOperations.ExtractSingleValueFromQuery(query, "rented")),
                    Order = QueryOperations.ExtactMetadataFromQueryField(query, "rented")
                } : null,
                DatePublished = QueryOperations.IfExists(query,"date") ? new Datepublished()
                {
                    Value = QueryOperations.CleanAndParseTimestampQuery(query),
                    Order = QueryOperations.ExtactMetadataFromQueryField(query, "date")
                } :null
            };
            return query_model;
        }
    }
}
