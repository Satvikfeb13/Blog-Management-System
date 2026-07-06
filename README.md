# 🚀 Blog Management System

A feature-rich **Blog Management Platform** built with **ASP.NET Core MVC (.NET 8)** that enables users to create, manage, and publish blogs while providing administrators with powerful moderation tools. The application includes role-based authentication, blog approval workflow, comment moderation, image uploads, category management, and a modern responsive UI.

![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-.NET%208-512BD4?style=for-the-badge&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-Language-239120?style=for-the-badge&logo=csharp)
![MySQL](https://img.shields.io/badge/MySQL-Database-4479A1?style=for-the-badge&logo=mysql)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?style=for-the-badge&logo=bootstrap)

---

# ✨ Features

## 👤 Authentication & Authorization

- ASP.NET Core Identity Authentication
- Role-Based Authorization (Admin & User)
- Secure Login & Registration
- Cookie-Based Authentication
- Protected Routes
- Custom User Profiles

---

## 📝 Blog Management

### User

- Create Blog Posts
- Edit Own Blogs
- Delete Own Blogs
- Save Drafts
- Submit Blogs for Review
- Upload Feature Images
- View Personal Blogs Dashboard

### Admin

- Create Blog Posts
- Edit Any Blog
- Delete Any Blog
- Approve Blogs
- Reject Blogs
- Publish Blogs
- Manage Categories

---

## 🔄 Blog Approval Workflow

```
Draft
   │
   ▼
Pending Review
   │
 ┌─┴───────────┐
 ▼             ▼
Published   Rejected
```

Only **Published** blogs are visible to the public.

---

## 💬 Comment System

- Add Comments
- Edit Own Comments
- Delete Own Comments
- Report Inappropriate Comments
- AJAX Comment Submission
- Comment Moderation

### Admin Moderation

- Hide Comments
- Restore Comments
- Delete Comments
- View Reported Comments

---

## 🔍 Search & Categories

- Blog Search
- Category Filtering
- Auto-Suggestion Search
- Rich Category Management

---

## 📊 Admin Dashboard

Dashboard includes:

- Total Users
- Total Blogs
- Published Blogs
- Pending Blogs
- Draft Blogs
- Rejected Blogs
- Categories
- Total Comments
- Reported Comments

---

## 🎨 Modern UI

- Dark Theme
- Responsive Design
- Bootstrap 5
- Glassmorphism Effects
- Font Awesome Icons
- AOS Animations
- TinyMCE Rich Text Editor
- Modern Cards
- Beautiful Dashboard

---

# 🛠 Tech Stack

| Technology | Used |
|------------|------|
| ASP.NET Core MVC (.NET 8) | Backend |
| C# | Programming Language |
| Entity Framework Core | ORM |
| MySQL | Database |
| ASP.NET Core Identity | Authentication |
| Bootstrap 5 | Frontend |
| HTML5 & CSS3 | UI |
| TinyMCE | Rich Text Editor |
| Font Awesome | Icons |
| AOS | Animations |

---

# 📁 Project Structure

```
BlogManagementSystem
│
├── Controllers
├── Models
├── ViewModels
├── Services
├── Data
├── Helpers
├── Views
├── wwwroot
│   ├── css
│   ├── js
│   ├── images
│
├── Migrations
├── Program.cs
└── appsettings.json
```

---

# ⚙️ Installation

## Clone Repository

```bash
git clone https://github.com/Satvikfeb13/Blog-Management-System.git
```

## Navigate

```bash
cd Blog-Management-System
```

## Configure Database

Update your connection string inside:

```json
appsettings.json
```

```json
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;database=blogdb;user=root;password=yourpassword"
}
```

---

## Apply Migrations

```bash
dotnet ef database update
```

---

## Run Application

```bash
dotnet run
```

---

# 🔑 Default Admin Credentials

```
Email:
admin@gmail.com

Password:
admin
```

---

# 📷 Screenshots

Add screenshots here:

- Home Page
- Blog Details
- Login
- Register
- My Blogs
- Admin Dashboard
- Pending Blogs
- Comment Moderation

---

# 🔒 Security Features

- Role-Based Authorization
- ASP.NET Identity
- CSRF Protection
- Server-Side Validation
- Ownership Validation
- Secure Image Upload
- Authentication Cookies

---

# 🚀 Future Enhancements

- Email Verification
- Forgot Password
- User Profile
- Bookmark Posts
- Like & Reactions
- Nested Comments
- Reading History
- Blog Analytics
- View Counter
- SEO Friendly URLs
- Pagination
- Related Posts
- AI Blog Suggestions
- Newsletter
- REST API
- Docker Support

---

# 👨‍💻 Author

**Satvik Patil**

GitHub: https://github.com/Satvikfeb13

---

# ⭐ If you found this project useful, consider giving it a star!

It helps support the project and encourages future improvements.
