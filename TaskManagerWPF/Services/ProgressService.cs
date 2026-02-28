using TaskManagerWPF.Models;

namespace TaskManagerWPF.Services;

public class ProgressService
{
    public double CalculateProgress(IEnumerable<TaskModel> tasks)
    {
        var list = tasks.ToList();

        if (list.Count == 0)
            return 0;

        var completed = list.Count(t => t.Status == StatusTask.Finalizat);

        return (double)completed / list.Count * 100.0;
    }
}