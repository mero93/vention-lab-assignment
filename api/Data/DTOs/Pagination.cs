namespace api.Data.DTOs
{
    public record PaginatedResult<T>(
        IEnumerable<T> Items,
        int TotalCount,
        int PageNumber,
        int PageSize
    )
    {
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    }

    public record QueryParams
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 20;
        public string? Title { get; init; }

        public int Skip => (Math.Max(1, PageNumber) - 1) * PageSize;
        public int Take => PageSize;
    }
}
