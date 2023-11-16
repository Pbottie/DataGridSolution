namespace DataGridProject.Interfaces;

public class NotificationService
{
    public event Action<string, NotificationType, int> OnNotify;

    public void Notify(string message, NotificationType notificationType, int delay = 2500)
    {
        OnNotify?.Invoke(message, notificationType, delay);
    }

}

public enum NotificationType
{
    None,
    Success,
    Error,
    Info,
    Warning,
    Primary,
}
public static class TimeoutHelper
{
    public static CancellationTokenSource SetTimeout(Action action, int timeout)
    {
        var cts = new CancellationTokenSource();
        var ct = cts.Token;
        Task.Delay(timeout).ContinueWith((Task task) =>
        {
            if (!ct.IsCancellationRequested)
            {
                action();
            }
        }, ct);
        return cts;
    }

    public static void ClearTimeout(CancellationTokenSource cts) => cts?.Cancel();

}
