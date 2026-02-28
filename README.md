TaskManager Desktop App

TaskManager este o aplicație desktop pentru gestionarea sarcinilor zilnice, dezvoltată în C# (.NET 8) folosind WPF și SQLite.
Aplicația permite organizarea eficientă a task-urilor, monitorizarea progresului și notificarea utilizatorului înainte de deadline.

Proiectul a fost realizat pentru a exersa arhitectura MVVM, lucrul cu baze de date locale și dezvoltarea unei interfețe reactive în WPF.

✨ Funcționalități principale

Creare, editare și ștergere task-uri

Organizare pe categorii și priorități

Filtrare și căutare în timp real

Sortare automată după urgență și deadline

Notificări automate înainte de expirarea termenului limită

Evidențiere vizuală pentru:

task-uri întârziate

task-uri din ziua curentă

Bară de progres ce indică procentul de task-uri finalizate (actualizare live)

Persistența datelor folosind SQLite (bază de date locală)

🧠 Arhitectură

Aplicația este construită folosind Model-View-ViewModel (MVVM) pentru separarea clară a responsabilităților.

Structura proiectului

Models → reprezintă datele (TaskModel + validare)

ViewModels → logica aplicației și binding către UI

Views → interfața WPF (XAML)

Services → servicii independente (notificări, progres, dialoguri)

Data → acces la baza de date (SQLite)

Repository Pattern → gestionarea operațiilor CRUD

Concepte implementate

Data Binding

INotifyPropertyChanged

IDataErrorInfo validation

ICommand (RelayCommand)

DispatcherTimer pentru notificări

Repository Pattern pentru accesul la date

Service Layer (ProgressService, DialogService)

🖥️ Tehnologii utilizate

C#

.NET 8

WPF (Windows Presentation Foundation)

SQLite

MVVM Pattern

🔔 Sistem de notificări

Aplicația verifică periodic task-urile și notifică utilizatorul cu câteva minute înainte de deadline.
Notificările sunt declanșate automat folosind DispatcherTimer.

📊 Monitorizare progres

Procentul de task-uri finalizate este calculat în timp real și afișat printr-un ProgressBar.
Bară se actualizează automat la:

modificarea statusului

adăugarea unui task

ștergerea unui task

🛠️ Instalare și rulare

Clonează repository-ul:

git clone https://github.com/adrianaalina/TaskManagerWPF.git

Deschide soluția în Visual Studio 2022+ sau Rider

Asigură-te că ai instalat:

.NET Desktop Development

.NET 8 SDK

Rulează aplicația (F5)

Baza de date SQLite va fi creată automat în folderul:

/bin/Debug/net8.0-windows/Data
📌 Scopul proiectului

Acest proiect a fost realizat pentru:

învățarea dezvoltării aplicațiilor desktop în WPF

implementarea arhitecturii MVVM

lucrul cu baze de date locale

dezvoltarea unei interfețe reactive și orientate pe utilizator

🔮 Dezvoltări viitoare

Export / Import task-uri (CSV / JSON)

Task-uri recurente

Statistici de productivitate

Dark Mode

Auto-save

👤 Autor

Rusu Adriana Alina
📧 adrirusu80@gmail.com

🔗 https://github.com/adrianaalina
