namespace DataGridProject.Interfaces;

public interface IFilter
{
    void RegisterObserver(IFilterObserver observer);
    void RemoveObserver(IFilterObserver observer);
    void NotifyAll();
}
