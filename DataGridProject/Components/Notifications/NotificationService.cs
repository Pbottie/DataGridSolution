namespace DataGridProject.Components;

public class NotificationService
{
    public event Action<string, NotificationType, int> OnNotify;

    public void Notify(string message, NotificationType notificationType, int delay = 5000)
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

public class Message
{
    public string Heading { get; set; }
    public string MessageContent { get; set; }
    public string BackgroundCssClass { get; set; }
    public Guid Id { get; private set; }
    public int Delay { get; set; }
    public NotificationType Type { get; set; }

    public Message(string heading, string messageContent, string backgroundCssClass, int delay, NotificationType type)
    {
        Heading = heading;
        MessageContent = messageContent;
        BackgroundCssClass = backgroundCssClass;
        Id = Guid.NewGuid();
        Delay = delay;
        Type = type;
    }
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