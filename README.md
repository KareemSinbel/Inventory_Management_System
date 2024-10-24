# Inventory Management System

![Inventory Management System](https://cashflowinventory.com/blog/wp-content/uploads/2023/02/inventory-management-system/728x90.webp)

## Overview

The **Inventory Management System** is a robust web-based application designed to efficiently manage products, suppliers, and stock levels. Built using ASP.NET Core MVC, Entity Framework, and a clean architecture, it provides businesses with tools to track stock levels, generate reports, and alert users when items are running low.

Key features include:

- **Product Management**: Add, edit, and delete products.
- **Supplier Management**: Manage suppliers linked to your products.
- **Stock Alerts**: Track low stock levels and receive warnings for out-of-stock items.
- **Reporting**: Generate PDF/Excel reports for products, stock levels, and suppliers.
- **Admin Control**: Admin-only features for advanced management and export functionality.
  
## Features

- **CRUD Operations** for products, suppliers, and stock levels.
- **Stock Level Monitoring** with alerts for low stock and out-of-stock items.
- **Report Generation** in multiple formats (PDF, Excel).
- **Role-based Authentication**: Admin-level features restricted to users with appropriate roles.
- **Factory Pattern** and **Repository Pattern** used to ensure maintainability and separation of concerns.
- **Error Handling** and user-friendly messages.
  
## Technologies Used

- **ASP.NET Core MVC** for the web application framework.
- **Entity Framework Core** for database access.
- **iTextSharp** for generating PDF reports.
- **EPPlus** for Excel export.
- **Microsoft SQL Server** as the database (can be configured to use other databases).
- **Bootstrap** for responsive design and UI styling.
