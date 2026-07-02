# 🚀 Advanced Real-time Chat System

<div align="center">

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-8.0-512BD4?style=for-the-badge&logo=dotnet)
![SignalR](https://img.shields.io/badge/SignalR-Real--Time-success?style=for-the-badge)
![Entity Framework](https://img.shields.io/badge/Entity_Framework_Core-8.0-purple?style=for-the-badge)
![SQL Server](https://img.shields.io/badge/SQL_Server-red?style=for-the-badge&logo=microsoftsqlserver)
![JWT](https://img.shields.io/badge/JWT-Authentication-orange?style=for-the-badge)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?style=for-the-badge&logo=bootstrap)

**A real-time chat platform built with ASP.NET Core 8, SignalR, Entity Framework Core, SQL Server, JWT Authentication, ASP.NET Identity, ASP.NET MVC, and Windows Forms, enabling secure, real-time communication across web and desktop clients.**

</div>

---

## 🎥 Demo

📹 **Watch the project demo here**

```
https://vimeo.com/1206461262
```

---

## 📸 Screenshots

### 🌐 MVC Web Application

<p align="center">
    <img src="ChatDesign/Client.png" width="100%">
</p>

---

### 🖥️ Windows Forms Desktop Client

<p align="center">
    <img src="ChatDesign/Desktop.png" width="70%">
</p>

---

## ✨ Features

- 🔐 JWT Authentication & ASP.NET Identity
- 💬 Real-time communication using SignalR
- 🏠 Create, join and delete chat rooms
- 📜 Persistent message history
- 👥 Online users tracking
- 🌐 ASP.NET MVC Web Client
- 🖥️ Windows Forms Desktop Client
- ⚡ Live synchronization across all connected clients

---

## 🏗️ Architecture

```text
                ASP.NET MVC Client
                        │
                        │ REST API + SignalR
                        │
Windows Forms Client ───┤
                        │
                  ASP.NET Core API
        (Controllers • SignalR • Identity)
                        │
             Entity Framework Core
                        │
                   SQL Server
```

---

## 🛠️ Technologies

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

## 🚀 Getting Started

```bash
git clone https://github.com/IslamElSaqqa/RealTimeChat-System-SignalR.git
```

Update the connection string inside:

```
API/appsettings.json
```

Apply migrations:

```bash
dotnet ef database update
```

Run:

- API
- MVC
- Windows Forms (optional)

---

## 📂 Project Folder Structure

```
## 📂 Project Structure

```text
AdvancedChat
│
├── API/                           # ASP.NET Core 8 Web API
│   ├── Controllers/               # REST API endpoints
│   ├── Data/                      # DbContext and EF Core configuration
│   ├── DTOs/                      # Data Transfer Objects
│   ├── Hubs/                      # SignalR Hub
│   ├── Migrations/                # Entity Framework Core migrations
│   ├── Models/                    # Domain entities
│   ├── Services/                  # Business logic & JWT services
│   ├── Program.cs
│   └── appsettings.json
│
├── Web/                           # ASP.NET MVC Client
│   ├── Controllers/               # MVC controllers
│   ├── DTOs/                      # API response/request models
│   ├── Models/                    # MVC models
│   ├── Services/                  # API communication services
│   ├── ViewModels/                # ViewModels for MVC views
│   ├── Views/                     # Razor Views
│   ├── wwwroot/                   # CSS, JavaScript, images
│   ├── Program.cs
│   └── appsettings.json
│
├── Desktop/                       # Windows Forms Client
│   ├── Forms/                     # Login & Chat forms
│   ├── Models/                    # Desktop models
│   ├── Services/                  # API & SignalR services
│   ├── Program.cs
│   └── Desktop.csproj
│
├── ChatDesign/                    # Screenshots & demo assets
│   ├── Client.png
│   └── Desktop.png
│
├── AdvancedChat.sln               # Visual Studio solution
├── README.md                      # Project documentation
└── .gitignore                     # Git ignore rules
```

```

---

---

## 👨‍💻 Author

### Islam Ashraf Mahmoud Elsaqqa

**Full-Stack .NET Developer | Software Engineer**

**ITI 9-Month Graduate – Professional Development & BI-Infused CRM Track**

<p align="left">
  <a href="https://www.linkedin.com/in/islam-elsaqqa/" target="_blank">
    <img src="https://img.shields.io/badge/LinkedIn-Islam%20Elsaqqa-0A66C2?style=for-the-badge&logo=linkedin&logoColor=white" alt="LinkedIn"/>
  </a>

  <a href="https://github.com/IslamElSaqqa" target="_blank">
    <img src="https://img.shields.io/badge/GitHub-IslamElSaqqa-181717?style=for-the-badge&logo=github&logoColor=white" alt="GitHub"/>
  </a>

  <a href="mailto:islamelsaqqa2002@gmail.com">
    <img src="https://img.shields.io/badge/Email-Contact%20Me-EA4335?style=for-the-badge&logo=gmail&logoColor=white" alt="Email"/>
  </a>
</p>

⭐ If you found this project useful, consider giving it a **Star** on GitHub!

