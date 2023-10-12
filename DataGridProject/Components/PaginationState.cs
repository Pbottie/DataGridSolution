namespace DataGridProject.Components;

public class PaginationState
{
    public int ItemsPerPage { get; set; } = 10;

    public int PageIndex { get; private set; }

    public int PageCount { get; private set; }

    public int TotalItemCount { get; private set; }

    public int LastPageIndex => (TotalItemCount - 1) / ItemsPerPage;

    public Task SetPageIndexAsync(int pageIndex)
    {
        PageIndex = pageIndex;
        return Task.CompletedTask;
    }

    internal Task SetTotalItemsAsync(int totalItems)
    {
        if (TotalItemCount == totalItems)
            return Task.CompletedTask;

        TotalItemCount = totalItems;

        if (PageIndex > 0 && PageIndex > LastPageIndex)
            return SetPageIndexAsync(LastPageIndex);

        return Task.CompletedTask;
    }
}
