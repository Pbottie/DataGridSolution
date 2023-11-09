namespace DataGridProject.Utils;

public static class DataUtils
{
    public static IEnumerable<PropertyInfo> GetItemsPropertyInfo<TItem>()
        => typeof(TItem)
        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
        .Where(x => x.CanRead);

}
