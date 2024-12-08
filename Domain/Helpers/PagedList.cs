namespace Domain.Helpers;

public class PagedList<T>(List<T> items, int pageNumber, int pageSize, int totalItemCount)
{
    public List<T> Items { get; } = items;
    public int PageNumber { get; } = pageNumber;
    public int PageSize { get; } = pageSize;
    public int TotalItemCount { get; } = totalItemCount;
    public int TotalPages => (int)Math.Ceiling((double)TotalItemCount / PageSize);
}