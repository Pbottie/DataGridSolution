using System.Linq.Expressions;
using System.Reflection;

namespace DataGridProject.Utils;

public static class DataGridUtils
{
    public static IEnumerable<PropertyInfo> GetItemsPropertyInfo<TItem>()
        => typeof(TItem)
        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
        .Where(x => x.CanRead);

    public static bool IsBeforeDate(this DateTime thisDate, DateTime date)
    {

        if (thisDate.Date.CompareTo(date.Date) > 0)
            return false;

        return true;
    }

    public static bool IsAfterDate(this DateTime thisDate, DateTime date)
    {
        if (thisDate.IsBeforeDate(date))
            return false;
        return true;
    }
}
