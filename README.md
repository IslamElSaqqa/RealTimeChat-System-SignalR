# 🚀 Advanced Real-time Chat System

<div align="center">

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-8.0-512BD4?style=for-the-badge&logo=dotnet)
![SignalR](https://img.shields.io/badge/SignalR-Real--Time-success?style=for-the-badge)
![Entity Framework Core](https://img.shields.io/badge/Entity_Framework_Core-8.0-purple?style=for-the-badge)
![SQL Server](https://img.shields.io/badge/SQL_Server-red?style=for-the-badge&logo=microsoftsqlserver)
![JWT](https://img.shields.io/badge/JWT-Authentication-orange?style=for-the-badge)
![ASP.NET Identity](https://img.shields.io/badge/ASP.NET_Identity-Security-blue?style=for-the-badge)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?style=for-the-badge&logo=bootstrap)
![Windows Forms](https://img.shields.io/badge/Windows_Forms-.NET_8-0078D6?style=for-the-badge)

**A real-time chat platform built with ASP.NET Core 8, SignalR, Entity Framework Core, SQL Server, JWT Authentication, ASP.NET Identity, ASP.NET MVC, and Windows Forms, enabling secure, real-time communication across web and desktop clients.**

</div>

---

# 🎥 Demo

[![Watch Demo](https://img.shields.io/badge/▶️-Watch_Demo-red?style=for-the-badge)](https://vimeo.com/1206461262)

---

# 📸 Screenshots

### 🌐 ASP.NET MVC Client

<p align="center">
    <img src="ChatDesign/Client.png" width="100%">
</p>

---

### 🖥️ Windows Forms Client

<p align="center">
    <img src="ChatDesign/Desktop.png" width="70%">
</p>

---

# ✨ Features

- 🔐 User Registration & Login using ASP.NET Identity
- 🛡️ JWT Authentication & Authorization
- 💬 Real-time messaging with SignalR
- 🏠 Create, Join and Delete Chat Rooms
- 📜 Persistent Chat History
- 👥 Live Online Users Tracking
- 🌐 ASP.NET MVC Web Client
- 🖥️ Windows Forms Desktop Client
- ⚡ Real-time synchronization across connected clients
- 🗄️ SQL Server database with Entity Framework Core

---

# 🏗️ Architecture

```text
                  ASP.NET MVC Client
                         │
                         │ REST API + SignalR
                         │
Windows Forms Client ────┤
                         │
                  ASP.NET Core Web API
       (Controllers • SignalR • Identity)
                         │
              Entity Framework Core
                         │
                    SQL Server
```

---

# 🛠️ Technologies

- ASP.NET Core 8 Web API
- ASP.NET MVC
- SignalR
- Entity Framework Core
- SQL Server
- ASP.NET Identity
- JWT Authentication
- Windows Forms (.NET 8)
- Bootstrap 5
- JavaScript

---

# 🚀 Getting Started

Clone the repository

```bash
git clone https://github.com/IslamElSaqqa/RealTimeChat-System-SignalR.git
```

Configure your SQL Server connection string inside:

```text
API/appsettings.json
```

Apply Entity Framework migrations

```bash
dotnet ef database update
```

Run the projects in the following order:

1. API
2. Web (MVC)
3. Desktop (Windows Forms) *(Optional)*

---

# 📂 Project Structure

```text
AdvancedChat
│
├── API/                           # ASP.NET Core 8 Web API
│   ├── Controllers/
│   ├── Data/
│   ├── DTOs/
│   ├── Hubs/
│   ├── Migrations/
│   ├── Models/
│   ├── Services/
│   ├── Program.cs
│   └── appsettings.json
│
├── Web/                           # ASP.NET MVC Client
│   ├── Controllers/
│   ├── DTOs/
│   ├── Models/
│   ├── Services/
│   ├── ViewModels/
│   ├── Views/
│   ├── wwwroot/
│   ├── Program.cs
│   └── appsettings.json
│
├── Desktop/                       # Windows Forms Client
│   ├── Forms/
│   ├── Models/
│   ├── Services/
│   ├── Program.cs
│   └── Desktop.csproj
│
├── ChatDesign/
│   ├── Client.png
│   └── Desktop.png
│
├── AdvancedChat.sln
├── README.md
└── .gitignore
```

---

# 📚 Learning Outcomes

This project demonstrates practical experience with:

- Building RESTful APIs using ASP.NET Core 8
- Real-time communication using SignalR
- JWT Authentication & ASP.NET Identity
- Entity Framework Core (Code First)
- SQL Server database design
- Layered Architecture
- Dependency Injection
- Multi-client application development
- Cross-platform communication between Web and Desktop clients

---

# 👨‍💻 About the Author

**Islam Ashraf Mahmoud Elsaqqa**

**Full-Stack .NET Developer | Software Engineer**

🎓 **ITI 9-Month Professional Program – Professional Development & BI-Infused CRM Track**

<p align="left">

<a href="https://www.linkedin.com/in/islam-elsaqqa/">
    <img src="https://img.shields.io/badge/LinkedIn-Islam%20Elsaqqa-0A66C2?style=for-the-badge&logo=linkedin&logoColor=white"/>
</a>

<a href="https://github.com/IslamElSaqqa">
    <img src="https://img.shields.io/badge/GitHub-IslamElSaqqa-181717?style=for-the-badge&logo=github&logoColor=white"/>
</a>

<a href="mailto:islamelsaqqa2002@gmail.com">
    <img src="https://img.shields.io/badge/Gmail-islamelsaqqa2002%40gmail.com-EA4335?style=for-the-badge&logo=gmail&logoColor=white"/>
</a>

</p>

---

<div align="center">

⭐ **If you found this project helpful, consider giving it a Star!**

</div>