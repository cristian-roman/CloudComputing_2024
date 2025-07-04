# CloudComputing_2024 – Cloud-Based .NET Applications and Services

This repository contains multiple cloud and network applications written in **C#** using **.NET Core**, including a Blazor Web UI and REST API server. It was developed for a course on Cloud Computing, with structured homework assignments exploring service-based architectures.

---

## 🧱 Structure Overview

```
CloudComputing_2024-main/
├── FirstHomework/                 # Network and API-based architecture
│   └── FirstHomework/            # Core logic (APIs, DB, Network)
├── SecondHomework/
│   ├── Calendar/                 # Blazor WebAssembly calendar app
│   └── MultiServer/             # RESTful multi-service API
```

---

## 🚀 Technologies Used

### ✅ FirstHomework
- **Language**: C# (.NET Core)
- **Focus**: Network programming and modular architecture
- **Folders**:
  - `APIs/`: Web API logic
  - `Network/`: Likely contains TCP/HTTP communication layers
  - `DB/`: Data access / repository layers

### ✅ SecondHomework – Calendar
- **Technology**: Blazor WebAssembly (ASP.NET Core)
- **Frontend**: `.razor` pages (SPA)
- **Structure**:
  - `Pages/`, `Layout/`, `Services/`, `Models/`
  - `wwwroot/` for static assets

### ✅ SecondHomework – MultiServer
- **Framework**: ASP.NET Core
- **Role**: Backend REST API with multiple controllers
- **Files**:
  - `Controllers/`: REST API endpoints
  - `appsettings.json`: Configuration
  - `Program.cs`, `Startup` logic

---

## 🧰 Requirements

To build and run the projects, you need:

- [.NET SDK 7.0+](https://dotnet.microsoft.com/download)
- Visual Studio 2022+ or Visual Studio Code (with C# plugin)

---

## ▶️ How to Run

### 🔧 FirstHomework

```bash
cd FirstHomework/FirstHomework
dotnet build
dotnet run
```

### 🌐 Calendar (Blazor App)

```bash
cd SecondHomework/Calendar
dotnet build
dotnet run
```

Then open the provided `localhost` link in your browser.

### 📡 MultiServer API

```bash
cd SecondHomework/MultiServer
dotnet build
dotnet run
```

Visit the API docs (if enabled) at:
```
http://localhost:5000/swagger
```

---

## 🎓 Educational Goals

- Understand client-server network architectures
- Deploy and manage Blazor SPA frontends
- Build and document RESTful APIs with ASP.NET Core
- Configure cloud-ready .NET apps using layered structures

---

## 🧪 Notes

- `.sln` files are included for opening each project in Visual Studio.
- Configuration files (`appsettings.json`) define environment-specific behavior.
- Folders like `bin/` and `obj/` are auto-generated on build and can be ignored in Git.
