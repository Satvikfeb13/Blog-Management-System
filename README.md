# Blog Management System

A **Blog Management System** built using **ASP.NET Core MVC** that allows administrators to create and manage blog posts while users can read posts and add comments.
The application implements **authentication, role-based authorization, image upload, category filtering, and commenting functionality** using ASP.NET Core Identity and Entity Framework Core.

---

## 🚀 Features

### 👤 User Features

* User **registration and login** using ASP.NET Core Identity
* View all blog posts
* Filter blog posts by **category**
* View detailed blog content
* Add **comments** to blog posts (only when logged in)
* Secure authentication with cookies

### 👑 Admin Features

* Create new blog posts
* Edit existing blog posts
* Delete blog posts
* Upload feature images for blog posts
* Manage content visibility on the platform

---

## 🛠 Tech Stack

* **Backend:** ASP.NET Core MVC (.NET 8)
* **Database:** MySQL
* **ORM:** Entity Framework Core
* **Authentication:** ASP.NET Core Identity
* **Frontend:** HTML, CSS, Bootstrap
* **Rich Text Editor:** TinyMCE
* **File Upload:** Image storage in `wwwroot/images`

---

## 📂 Project Structure

```
Blog-Management-System
│
├── Controllers
│   ├── AuthController.cs
│   ├── PostController.cs
│   ├── HomeController.cs
│   └── AccountController.cs
│
├── Data
│   └── AppDbContext.cs
│
├── Models
│   ├── Post.cs
│   ├── Category.cs
│   ├── Comment.cs
│   └── ErrorViewModel.cs
│
├── Models/ViewModels
│   ├── RegisterViewModel.cs
│   ├── LoginViewModel.cs
│   ├── PostViewModel.cs
│   └── EditViewModel.cs
│
├── Helper
│   └── RemoveHtmlTagHelper.cs
│
├── Views
│   ├── Auth
│   ├── Post
│   ├── Home
│   └── Shared
│
├── wwwroot
│   ├── css
│   ├── js
│   └── images
│
├── Migrations
│
└── Program.cs
```

---

## 🔐 Authentication & Authorization

The application uses **ASP.NET Core Identity** with role-based access control.

| Role      | Permissions                                   |
| --------- | --------------------------------------------- |
| **Admin** | Create, edit, and delete blog posts           |
| **User**  | Register, login, read posts, and add comments |

An **Admin user is automatically created during application startup**:

```
Email: admin@gmail.com
Password: admin
```

---

## 📸 Image Upload

Admins can upload feature images while creating or editing blog posts.
Uploaded images are stored inside:

```
wwwroot/images
```

Each uploaded image is saved with a **unique GUID filename**.

---

## 🗂 Database Entities

### Post

* Title
* Content
* Author
* FeatureImagePath
* PublishedDate
* CategoryId

### Category

* Name
* Description

### Comment

* UserName
* CommentDate
* Content
* PostId

Relationships:

* One **Category → Many Posts**
* One **Post → Many Comments**

---

## ⚙️ Setup Instructions

### 1️⃣ Clone the Repository

```
git clone https://github.com/Satvikfeb13/Blog-Management-System.git
```

---

### 2️⃣ Navigate to the Project

```
cd Blog-Management-System
```

---

### 3️⃣ Configure Database

Update your **connection string** in:

```
appsettings.json
```

Example:

```
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;database=blogdb;user=root;password=yourpassword"
}
```

---

### 4️⃣ Apply Migrations

```
update-database
```

or

```
dotnet ef database update
```

---

### 5️⃣ Run the Application

```
dotnet run
```

Then open in browser:

```
https://localhost:5001
```

---

## 💬 Comment System

Users can comment on blog posts using an **AJAX-based comment submission**.
Comments are dynamically added to the page without refreshing.

---

## 🧹 Helper Utilities

### RemoveHtmlTagHelper

Removes HTML tags from blog content when displaying preview text in the blog list.

Example:

```
RemoveHtmlTagHelper.removehtmltag(post.Content)
```

---

## 📌 Future Improvements

* Blog **search functionality**
* Pagination for blog posts
* User profile page
* Post **like/reaction system**
* Admin dashboard
* Comment moderation

---

## 👨‍💻 Author

**Satvik Patil**

GitHub:
https://github.com/Satvikfeb13

---

## 📜 License

This project is created for **learning and educational purposes**.
