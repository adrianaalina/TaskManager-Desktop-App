using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Data.Common;
using System.Data.SQLite;
using System.Diagnostics;
using System.Windows.Threading;
using System.Windows.Input;
using TaskManagerWPF.Models;
using TaskManagerWPF.Services;
using TaskManagerWPF.Data;
using TaskManagerWPF.ViewModels.Base;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
namespace TaskManagerWPF.ViewModels;


public class TaskViewModel : BaseViewModel
{
    private readonly TaskRepository _repo = new TaskRepository();
    public ICommand AddCommand { get; private set; }
    public ICommand DeleteCommand { get; private set; }
    public Array Statusuri => Enum.GetValues(typeof(StatusTask));
    public Array Categorii => Enum.GetValues(typeof(CategoriiTask));
    public Array Prioritati => Enum.GetValues(typeof(PrioritateTask));
    public List<FilterOption<StatusTask>> StatusuriFiltru { get; }
    public List<FilterOption<CategoriiTask>> CategoriiFiltru { get; }
    public List<FilterOption<PrioritateTask>> PrioritatiFiltru { get; }

    private ObservableCollection<TaskModel> _taskuriC = new();
    private DispatcherTimer _notificationTimer;

    public ICollectionView TaskuriView {get; private set; }

    private readonly IDialogService _dialogService;
    private string? _searchText;

    public string? SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            OnPropertyChanged();
            TaskuriView?.Refresh();
        }
    }
    
    private FilterOption<StatusTask>? _selectedStatus;
    public FilterOption<StatusTask>? SelectedStatus
    {
        get => _selectedStatus;
        set
        {
            _selectedStatus = value;
            OnPropertyChanged();
            TaskuriView.Refresh();
        }
    }
    private FilterOption<CategoriiTask>? _selectedCategorie;
    public FilterOption<CategoriiTask>? SelectedCategorie
    {
        get => _selectedCategorie;
        set
        {
            _selectedCategorie = value;
            OnPropertyChanged();
            TaskuriView.Refresh();
        }
    }
    private FilterOption<PrioritateTask>? _selectedPrioritate;
    public FilterOption<PrioritateTask>? SelectedPrioritate
    {
        get => _selectedPrioritate;
        set
        {
            _selectedPrioritate = value;
            OnPropertyChanged();
            TaskuriView.Refresh();
        }
    }
    public ObservableCollection<TaskModel> TaskuriC
    {
        get => _taskuriC;
        set
        {
            _taskuriC = value;
            OnPropertyChanged();
        }
    }
    
    

    //task curent pentru editare sau adaugare
    private TaskModel? _selectedTask;

    public TaskModel? SelectedTask
    {
        get => _selectedTask;
        set
        {
            _selectedTask = value;
            OnPropertyChanged();
           
           
            if (_selectedTask != null)
            {
                CurrentTask = new TaskModel
                {
                    Id = _selectedTask.Id,
                    Titlu = _selectedTask.Titlu,
                    Descriere = _selectedTask.Descriere,
                    Deadline = _selectedTask.Deadline,
                    Categorie = _selectedTask.Categorie,
                    Status = _selectedTask.Status,
                    Prioritate = _selectedTask.Prioritate
                };
            } 
            OnPropertyChanged(nameof(Ora));
            OnPropertyChanged(nameof(Minut));
            CommandManager.InvalidateRequerySuggested();
        }
    }

    private TaskModel _currentTask = new TaskModel();

    public TaskModel CurrentTask
    {
        get => _currentTask;
        set
        {
            if (_currentTask != null)
                _currentTask.PropertyChanged -= CurrentTask_PropertyChanged;

            _currentTask = value;

            if (_currentTask.Deadline == null)
                _currentTask.Deadline = DateTime.Today;
               
            _currentTask.PropertyChanged += CurrentTask_PropertyChanged;

            OnPropertyChanged();
           }
    }

    private void CurrentTask_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        CommandManager.InvalidateRequerySuggested();
        if (e.PropertyName == nameof(TaskModel.Deadline) && CurrentTask.Deadline != null)
        {
            var selected = CurrentTask.Deadline.Value;
            var now = DateTime.Now;

            if (selected.Date == now.Date && selected.Hour == 0 && selected.Minute == 0)
            {
                CurrentTask.Deadline = now.AddMinutes(10);
            }

            OnPropertyChanged(nameof(Ora));
            OnPropertyChanged(nameof(Minut));
        }
    }



    //selectare deadline
    public int Ora
    {
        get => CurrentTask?.Deadline?.Hour ?? 0;
        set
        {
            if (CurrentTask?.Deadline == null) return;

            var d = CurrentTask.Deadline.Value;

            CurrentTask.Deadline = new DateTime(
                d.Year,
                d.Month,
                d.Day,
                value,
                d.Minute,
                0);

            OnPropertyChanged(nameof(Ora));
            OnPropertyChanged(nameof(CurrentTask));
            CommandManager.InvalidateRequerySuggested();
        }
    }

    public int Minut
    {
        get => CurrentTask?.Deadline?.Minute ?? 0;
        set
        {
            if (CurrentTask?.Deadline == null) return;

            var d = CurrentTask.Deadline.Value;

            CurrentTask.Deadline = new DateTime(
                d.Year,
                d.Month,
                d.Day,
                d.Hour,
                value,
                0);

            OnPropertyChanged(nameof(Minut));
            OnPropertyChanged(nameof(CurrentTask));
            CommandManager.InvalidateRequerySuggested();
        }
    }
    

    //constructor
    public TaskViewModel()
    {
        _dialogService = new DialogService();
        
        CurrentTask = new TaskModel(); 
        
        TaskuriView=CollectionViewSource.GetDefaultView(TaskuriC);
        TaskuriView.Filter = FiltruTaskuri;
        TaskuriView.SortDescriptions.Add(
            new SortDescription(nameof(TaskModel.UrgentaSortare), ListSortDirection.Ascending));

        TaskuriView.SortDescriptions.Add(
            new SortDescription(nameof(TaskModel.PrioritateSortare), ListSortDirection.Ascending));

        TaskuriView.SortDescriptions.Add(
            new SortDescription(nameof(TaskModel.Deadline), ListSortDirection.Ascending));
        
        foreach (var t in _repo.GetAll())
            TaskuriC.Add(t);
        
        StatusuriFiltru = new List<FilterOption<StatusTask>>
        {
            new FilterOption<StatusTask>{ Display="Toate", Value= null }
        };

        StatusuriFiltru.AddRange(
            Enum.GetValues(typeof(StatusTask))
                .Cast<StatusTask>()
                .Select(s => new FilterOption<StatusTask>
                {
                    Display = s.ToString(),
                    Value = s
                }));

        CategoriiFiltru = new List<FilterOption<CategoriiTask>>
        {
            new FilterOption<CategoriiTask>{ Display="Toate", Value=null }
        };
        
        CategoriiFiltru.AddRange(
            Enum.GetValues(typeof(CategoriiTask))
                .Cast<CategoriiTask>()
                .Select(c => new FilterOption<CategoriiTask>
                {
                    Display = c.ToString(),
                    Value = c
                }));


        PrioritatiFiltru = new List<FilterOption<PrioritateTask>>
        {
            new FilterOption<PrioritateTask>{ Display="Toate", Value=null }
        };

        PrioritatiFiltru.AddRange(
            Enum.GetValues(typeof(PrioritateTask))
                .Cast<PrioritateTask>()
                .Select(p => new FilterOption<PrioritateTask>
                {
                    Display = p.ToString(),
                    Value = p
                }));
        
        AddCommand = new RelayCommand(_ => SaveTask(), _ => IsTaskValid());
        DeleteCommand = new RelayCommand(_ => DeleteSelected(), _ => SelectedTask != null);

        CommandManager.InvalidateRequerySuggested();
        _notificationTimer= new DispatcherTimer();
        _notificationTimer.Interval = TimeSpan.FromMinutes(1);
        _notificationTimer.Start();
    }

    //Stergere
    private void DeleteSelected()
    {
        if(SelectedTask==null) return;
        
        bool rezultat = _dialogService.ShowConfirmation(
            $"Sigur vrei sa stergi task-ul \"{SelectedTask.Titlu}\"?",
            "Confirmare");

        if (!rezultat) return;
        _repo.Delete(SelectedTask.Id);
        
        TaskuriC.Remove(SelectedTask);
        SelectedTask = null;
        
        CurrentTask = new TaskModel { Deadline = DateTime.Now };

        OnPropertyChanged(nameof(Ora));
        OnPropertyChanged(nameof(Minut));
        

    }
    

    
    //Adaugare si actualizare
    public void SaveTask()
    {
        if (CurrentTask.Id == 0)
        {
            CurrentTask.Id = _repo.Insert(CurrentTask);
            TaskuriC.Add(new TaskModel
            {
                Id = CurrentTask.Id,
                Titlu = CurrentTask.Titlu,
                Descriere = CurrentTask.Descriere,
                Deadline = CurrentTask.Deadline,
                Categorie = CurrentTask.Categorie,
                Status = CurrentTask.Status,
                Prioritate = CurrentTask.Prioritate
            });
        }
        else
        {
            _repo.Update(CurrentTask);
            var existing = TaskuriC.FirstOrDefault(t => t.Id == CurrentTask.Id);
            if (existing != null)
            {
                existing.Titlu = CurrentTask.Titlu;
                existing.Descriere = CurrentTask.Descriere;
                existing.Deadline = CurrentTask.Deadline;
                existing.Categorie = CurrentTask.Categorie;
                existing.Status = CurrentTask.Status;
                existing.Prioritate = CurrentTask.Prioritate;
            }
        }
        // RESET FORMULAR
        TaskuriView.Refresh();
        SelectedTask = null;
        CurrentTask = new TaskModel { Deadline = DateTime.Now };
        OnPropertyChanged(nameof(CurrentTask));
        OnPropertyChanged(nameof(Ora));
        OnPropertyChanged(nameof(Minut));

    }


    //Validare
    private bool IsTaskValid()
    {
        if (CurrentTask == null)
            return false;

        if (!string.IsNullOrEmpty(CurrentTask[nameof(CurrentTask.Titlu)]))
            return false;

        if (!string.IsNullOrEmpty(CurrentTask[nameof(CurrentTask.Deadline)]))
            return false;

        if (!string.IsNullOrEmpty(CurrentTask[nameof(CurrentTask.Descriere)]))
            return false;

        return true;
    }
    

    private bool FiltruTaskuri(object obj)
    {
        if (obj is not TaskModel task)
            return false;
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            if (task.Titlu == null ||
                !task.Titlu.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                return false;
        }
        bool noStatus = SelectedStatus?.Value == null;
        bool noCat = SelectedCategorie?.Value == null;
        bool noPrio = SelectedPrioritate?.Value == null;

        if (noStatus && noCat && noPrio)
            return true;

        if (SelectedStatus?.Value != null && task.Status != SelectedStatus.Value)
            return false;
        
        if (SelectedCategorie?.Value != null && task.Categorie != SelectedCategorie.Value)
            return false;
        
        if (SelectedPrioritate?.Value != null && task.Prioritate != SelectedPrioritate.Value)
            return false;

        return true;
    }


    
}
   

    