namespace TaskManagerWPF.ViewModels;
using System.Collections.ObjectModel;
using TaskManagerWPF;
using System.ComponentModel;

public class MainViewModel :INotifyPropertyChanged
{
    private TaskViewModel _taskViewModel;

    public TaskViewModel TaskVM
    {
        get => _taskViewModel;
        set
        {
            _taskViewModel = value;
            OnPropertyChanged(nameof(TaskVM));
        }
    }

    public MainViewModel()
    {
        TaskVM = new TaskViewModel();
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}