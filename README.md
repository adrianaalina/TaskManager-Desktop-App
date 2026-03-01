# 🚀 TASK MANAGER — Desktop Productivity Application
### WPF • .NET 8 • SQLite • MVVM Architecture

Aplicație desktop pentru organizarea și urmărirea sarcinilor zilnice, dezvoltată în **C# (.NET 8)** utilizând **WPF** și **SQLite**.

Proiectul simulează un produs real de productivitate (similar Microsoft To Do / Todoist) și pune accent atât pe **arhitectură software corectă**, cât și pe **experiența utilizatorului**: notificări automate, progres în timp real și actualizare live a interfeței.

---

## 📸 Screenshots
*(vor fi adăugate după finalizarea designului UI)*

---

## ✨ Funcționalități

### 📝 Gestionare Task-uri
- Creare, editare și ștergere task-uri
- Deadline cu dată și oră
- Statusuri: `Neinceput`, `InLucru`, `Finalizat`
- Categorii și priorități
- Editare directă din interfață

### 🔔 Notificări inteligente
- Verificare automată periodică a deadline-urilor
- Avertizare înainte de expirarea unui task
- Prevenirea notificărilor duplicate

### 🔍 Filtrare & Căutare
- Căutare în timp real după titlu
- Filtrare după:
  - status
  - categorie
  - prioritate
- Sortare automată:
  - task întârziat
  - task de azi
  - task viitor

### 📊 Monitorizare progres
- Progress bar calculat dinamic
- Actualizare instant când un task este finalizat
- Sistem de evenimente între model și UI

### 💾 Persistența datelor
- Salvare permanentă folosind **SQLite**
- Baza de date se creează automat la prima rulare
- Încărcare automată a taskurilor la pornirea aplicației

### 🎨 Interfață utilizator
- Interfață WPF bazată pe Styles & ControlTemplates
- Card layout pentru taskuri
- Highlight vizual pentru:
  - taskuri întârziate
  - taskuri din ziua curentă
- Feedback vizual la hover și selectare

---

## 🧱 Arhitectură

Aplicația este construită folosind modelul **MVVM (Model-View-ViewModel)**:

| Layer | Responsabilitate |
|------|------|
| **Model** | Structura datelor și validare (`TaskModel`, `IDataErrorInfo`) |
| **ViewModel** | Logica aplicației și binding-uri UI |
| **View** | Interfața WPF și stilizare |
| **Repository** | Operații CRUD SQLite |
| **Services** | Notificări și calcul progres |

---

## 🛠️ Tehnologii utilizate

- **C#**
- **.NET 8**
- **WPF**
- **SQLite**
- **MVVM Pattern**
- **Data Binding & Commands**
- **DispatcherTimer**

---

## ⚙️ Instalare

1. Clonează repository-ul:
```bash
git clone https://github.com/adrianaalina/TaskManagerWPF.git
