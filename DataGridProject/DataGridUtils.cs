using System.Reflection;

namespace DataGridProject;

internal static class DataGridUtils
{
    public static IEnumerable<PropertyInfo> GetItemsPropertyInfo<TItem>()
        => typeof(TItem)
        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
        .Where(x => x.CanRead);
}
