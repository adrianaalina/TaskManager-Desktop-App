using System.Windows.Threading;
using TaskManagerWPF.Models;

namespace TaskManagerWPF.Services;

public class NotificationService
{
    private readonly DispatcherTimer _timer = new();
    private readonly Func<IEnumerable<TaskModel>> _getTasks;

    public event Action<TaskModel>? TaskDueSoon;

    public NotificationService(Func<IEnumerable<TaskModel>> getTasks)
    {
        _getTasks = getTasks;

        _timer.Interval = TimeSpan.FromMinutes(1);
        _timer.Tick += CheckTasks;
    }

    public void Start() => _timer.Start();
    public void Stop() => _timer.Stop();

    private void CheckTasks(object? sender, EventArgs e)
    {
        var now = DateTime.Now;

        foreach (var task in _getTasks())
        {
            if (task.Deadline == null)
                continue;

            var remaining = task.Deadline.Value - now;

            // notificare cu 10 minute înainte
            if (remaining.TotalMinutes <= 10 && remaining.TotalMinutes > 0)
            {
                TaskDueSoon?.Invoke(task);
            }
        }
    }
}