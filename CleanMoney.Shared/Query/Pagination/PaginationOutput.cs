namespace CleanMoney.Shared
{
    public class PaginationOutput
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; private set; }
        public int StartItem { get; private set; }
        public int EndItem { get; private set; }
        public bool IsLastPage { get; private set; }

        public void Calculate()
        {
            TotalPages = PageSize > 0
                ? (int)Math.Ceiling((double)TotalItems / PageSize)
                : 0;

            var maxPage = TotalPages == 0 ? 1 : TotalPages;
            PageNumber = Math.Min(Math.Max(PageNumber, 1), maxPage);

            if (TotalItems == 0)
            {
                StartItem = 0;
                EndItem = 0;
            }
            else
            {
                StartItem = (PageNumber - 1) * PageSize + 1;
                EndItem = Math.Min(PageNumber * PageSize, TotalItems);
            }

            IsLastPage = PageNumber >= TotalPages;
        }
    }

}
