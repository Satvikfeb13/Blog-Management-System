# 🚀 ASP.NET Core Blog Management System

A modern, robust, and visually stunning Blog Management platform built with **ASP.NET Core MVC 8**. It features a comprehensive server-side moderation workflow, role-based authorization (Admin & User), a beautiful glassmorphism dark theme, and interactive comment reporting.

---

## ✨ Key Features

### 1. 📝 User-Generated Content & Approval Workflow
- **Roles:** Two primary roles — `Admin` and `User`.
- **Authoring:** Authenticated users can write blogs using a rich-text WYSIWYG editor (TinyMCE) and upload feature images.
- **Custom Usernames:** Authors register with a unique Username which securely and automatically tags their posts and comments.
- **Statuses:** Blogs go through a strict lifecycle: `Draft` ➔ `Pending Review` ➔ `Published` or `Rejected`.
- **Public Visibility:** Only strictly `Published` posts are visible on the public homepage.

### 2. 🛡️ Admin Moderation & Dashboards
- **Admin Dashboard:** Real-time metrics tracking Total Users, Total Comments, Categories, and segmented Blog Submissions (Draft, Pending, Published, Rejected).
- **Pending Submissions:** Admins can review pending blogs and either **Approve** (publishes immediately) or **Reject** (with an optional feedback reason sent to the author).
- **Comment Moderation:** Users can "Report" offensive comments. Admins have a dedicated **Reported Comments** queue to Hide, Restore, or permanently Delete flagged comments.

### 3. 🔍 Smart Search & Discovery
- **HTML5 Auto-suggest Search:** The main search bar features a native, highly-optimized dropdown that auto-suggests categories (e.g., typing "Tech" suggests "Technology").
- **Rich Categorization:** Blogs are segmented into popular pre-seeded categories like *Personal Finance, Artificial Intelligence, Travel, Food & Cooking, Productivity, and Business*.

### 4. 👤 User Experience (UX) & Security
- **My Blogs Dashboard:** A dedicated space for users to track their submissions and read admin feedback on rejected posts.
- **Secure Deletion UI:** A premium, centrally-aligned warning modal ensures users don't accidentally delete their content without strict confirmation.
- **Premium UI:** A consistent, high-contrast dark theme utilizing modern `glassmorphism` aesthetic, custom CSS variables, and Bootstrap 5.

---

## 🛠️ Technology Stack

- **Backend:** C#, ASP.NET Core MVC 8
- **Database:** MySQL, Entity Framework Core (`Pomelo.EntityFrameworkCore.MySql`)
- **Authentication:** ASP.NET Core Identity (Cookie-based auth, Role management)
- **Frontend:** HTML5, CSS3, Bootstrap 5.3, AOS Animations, FontAwesome
- **Editor:** TinyMCE

---

## 🚀 Getting Started

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [MySQL Server](https://dev.mysql.com/downloads/mysql/)
- Visual Studio 2022 (Recommended) or VS Code

### Installation & Setup

1. **Clone the repository** (or open the solution in Visual Studio).
2. **Configure the Database:**
   - Open `appsettings.json`.
   - Update the `DefaultConnection` string with your MySQL credentials.
3. **Apply Database Migrations:**
   - Open the Package Manager Console (PMC) or terminal and run:
     ```bash
     dotnet ef database update
     ```
4. **Run the Application:**
   - Press `F5` in Visual Studio or run `dotnet run` in your terminal.
   - The application will automatically seed the initial roles (`Admin`) and a default Admin account if the database is empty.

### 🔑 Default Credentials

To test the Admin workflow (approving posts, moderating comments, viewing dashboards), log in using the auto-seeded admin account:

- **Email:** `admin@gmail.com`
- **Password:** `admin`

---

## 📁 Project Structure Highlights

- `/Models`: Entity classes (Post, Comment, Category, CommentReport, ApplicationUser) & Enums.
- `/Controllers`:
  - `PostController`: Handles blog CRUD, smart search functionality, ownership logic, and user dashboards.
  - `AdminController`: Secured endpoint `[Authorize(Roles = "Admin")]` for dashboard and moderation views.
- `/Services`: `IPostService` & `PostService` containing the core business logic and asynchronous database queries.
- `/Views`: Razor pages meticulously styled with our custom `site.css` dark theme.

---

## 🧹 Helper Utilities

### RemoveHtmlTagHelper
Removes HTML tags from blog content when displaying preview text in the blog list.

Example:
```csharp
RemoveHtmlTagHelper.removehtmltag(post.Content)
```

---

## 🧪 Git Management

This repository includes a strict `.gitignore` designed for ASP.NET Core and JetBrains/Visual Studio development. Local configuration files (such as `appsettings.Development.json` and `.env`) are explicitly ignored to prevent sensitive database secrets from leaking.

---

## 👨‍💻 Author

**Satvik Patil**

GitHub:
https://github.com/Satvikfeb13

---

## 📜 License

This project is created for **learning and educational purposes**.
