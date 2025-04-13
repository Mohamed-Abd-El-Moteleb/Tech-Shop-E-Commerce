# 🛒 Tech-Shop E-Commerce Website

Tech-Shop is a full-featured e-commerce web application built with **ASP.NET Core MVC**, supporting both **Admin** and **Customer** roles. It includes product management, order processing, Stripe payments, and a dynamic shopping experience.

---

## 🌟 Features

### 👤 Customer Area
- Browse products by type, special tag, color, availability
- Product search with dynamic filtering (AJAX/jQuery)
- Shopping cart using session storage
- Checkout system with Stripe integration
- Order history

### 🔐 Admin Area
- Login/Registration for Admins (with role-based access)
- Full Product CRUD (create, edit, delete, image upload)
- Order management and order detail views
- Dashboard with revenue, orders, and product stats
- Admin-only access with role-based authorization

---

## 🧰 Tech Stack

- **ASP.NET Core MVC**
- **Entity Framework Core (EF Core)**
- **SQL Server / Somee SQL Hosting**
- **Stripe** for secure payments
- **Session-based cart** system
- **Repository Pattern** for data access
- **Identity** for authentication and role management
- **Bootstrap 5**, jQuery, AJAX for UI & interactivity

---
📁 **Project Structure**
```
Shop/
├── Areas/
│   ├── Admin/
│   └── Customer/
├── Controllers/
├── Data/
├── Models/
├── Repositories/
├── ViewModels/
├── Views/
├── wwwroot/
└── Program.cs
```


## 🖥️ Run Locally

**Clone the repository**

```bash
git clone https://github.com/Mohamed-Abd-El-Moteleb/Tech-Shop.git
```
---

## 🚀 Getting Started

### ⚙️ Update the Connection String

In `appsettings.json`:

``For local SQL Server:``
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=Shop;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```
💾 **Apply Migrations** (for local development)
```
Update-Database
```
🔐 **Admin Access**
```
Auto-seeded by DbSeeder.cs:

Email: Midoshaaban95@gmail.com
Password: Admin123##
```

💳 **Stripe Integration**
`
Stripe is integrated using Stripe.NET SDK.
`
Add to appsettings.json:
```
"Stripe": {
  "SecretKey": "your_secret_key",
  "PublishableKey": "your_publishable_key"
}
```

Use test card:
````
4242 4242 4242 4242
Exp: 12/34 | CVC: 123
````




👨‍💻 ***Developed By***

Mohamed Shaban

📧 Mohammed.shabaan.099@gmail.com

📍 Egypt


