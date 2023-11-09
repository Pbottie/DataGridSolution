namespace DataGridProject.Services;
public class FilterService : IFilter
{
    private List<IFilterObserver> _observers = new List<IFilterObserver>();
    public void NotifyAll()
    {
        foreach (var observer in _observers)
        {
            observer.Update();
        }
    }

    public void RegisterObserver(IFilterObserver observer)
    {
        _observers.Add(observer);
    }

    public void RemoveObserver(IFilterObserver observer)
    {
        _observers.Remove(observer);
    }
}
