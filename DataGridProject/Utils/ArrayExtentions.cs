namespace DataGridProject.Utils;

public static class ArrayExtensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
    {
        if (collection != null)
        {
            return !collection.Any();
        }

        return true;
    }
}
