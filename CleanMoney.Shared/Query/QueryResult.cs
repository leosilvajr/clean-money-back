namespace CleanMoney.Shared
{
    public class QueryResult<T>
    {
        public PaginationOutput? Pagination { get; set; }
        public Filter? Filter { get; set; }
        public Ordering? Ordering { get; set; }
        public string? Search { get; set; }
        public bool? ShowDeleted { get; set; }

        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();

        public QueryResult() { }

        public QueryResult(QueryParams queryParams)
        {
            Pagination = new PaginationOutput
            {
                PageNumber = queryParams.Pagination?.PageNumber ?? 1,
                PageSize = queryParams.Pagination?.PageSize ?? 10
            };

            Filter = queryParams.Filter;
            Ordering = queryParams.Ordering;
            Search = queryParams.Search;
            ShowDeleted = queryParams.ShowDeleted;
        }
    }
}
