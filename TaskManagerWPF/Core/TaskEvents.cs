namespace TaskManagerWPF.Core;

using TaskManagerWPF.Models;

public static class TaskEvents
{
    public static event Action<TaskModel>? TaskAdded;
    public static event Action<TaskModel>? TaskUpdated;
    public static event Action<int>? TaskDeleted;

    public static void RaiseTaskAdded(TaskModel task)
        => TaskAdded?.Invoke(task);

    public static void RaiseTaskUpdated(TaskModel task)
        => TaskUpdated?.Invoke(task);

    public static void RaiseTaskDeleted(int id)
        => TaskDeleted?.Invoke(id);
}