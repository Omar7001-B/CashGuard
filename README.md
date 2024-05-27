# Cash Guard 🛡️💰
[![Coin Guard](https://github.com/Omar7001-B/CoinGuard/assets/115028809/b0fc971c-fab9-4070-9de7-4f62756da822)](https://github.com/Omar7001-B/CoinGuard) 

## Table of Contents
- [Purpose](#purpose) 🎯
- [Project Description](#project-description) 📝
- [Key Features](#key-features) ✨
- [Structure](#structure) 🏗️
- [Usage](#usage) 🚀
- [Dependencies](#dependencies) 🛠️
- [Installation Instructions](#installation-instructions) ⚙️
- [Usage Instructions](#usage-instructions) 📖
- [Contribution](#contribution) 🤝

## Purpose 🎯
The primary goal of Cash Guard is to track your income and expenses.

## Project Description 📝
Cash Guard is a web application developed using ASP.NET MVC C#. With Cash Guard, users can:

- **Register:** Create an account by entering their details and uploading a profile picture.
- **Create Categories:** Organize their finances by creating different categories.
- **Add Transactions:** Record financial transactions by specifying the amount, date, note, and type (Income/Expense).
- **Track Money:** Monitor their income and expenses with summaries, latest transaction lists, and charts displaying financial data over time.

The application provides a summary of all expenses and income, displays the latest transactions, and shows charts to illustrate the data over time.

## Key Features ✨
- **User-Friendly and Fast:** The interface is intuitive and responsive.
- **User Data Storage:** Securely stores user data.
- **Time and Accounting Efficiency:** Saves time and simplifies accounting tasks.
- **Beautiful Interface:** The design is visually appealing and easy on the eyes.
- **Analytics and Filtering:** Offers extensive analytics and filtering options.
- **History Tracking:** Allows users to monitor their activities and transactions.
- **Password Encryption:** Ensures security by encrypting passwords in the database.
- **Delete Options:** Users can delete any category or transaction.

##  Structure 🏗️

```
└── CashGuard/
    ├── Controllers/
    │   ├── LoginController.cs
    │   ├── HomeController.cs
    │   ├── TransactionController.cs
    │   ├── AnalyticsController.cs
    │   ├── CategoryController.cs
    │   ├── AccountSettingsController.cs
    │   └── GeneralSettingsController.cs
    ├── Models/
    │   ├── Category.cs
    │   ├── Transaction.cs
    │   ├── GeneralSettings.cs
    │   ├── HistoryItem.cs
    │   └── User.cs
    └── Views/
        ├── Home/
        ├── Category/
        ├── GeneralSettings/
        ├── AccountSettings/
        ├── Analytics/
        └── Login/
```

## Usage 🚀
Cash Guard can be accessed easily through the following link: [Cash Guard GitHub Repository](https://github.com/Omar7001-B/CashGuard)

<details>
<summary>Dependencies 🛠️</summary>

The project utilizes the following packages, all at version 6:
- Microsoft.VisualStudio.Web.CodeGeneration.Design
- Microsoft.EntityFrameworkCore.Tools
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Sqlite
- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore
- EntityFramework
</details>

<details>
<summary>Installation Instructions ⚙️</summary>

To install and set up Cash Guard, follow these steps:
1. Clone the repository from GitHub.
   git clone https://github.com/Omar7001-B/CashGuard.git
2. Ensure you have .NET 6.0 installed on your machine.
3. Navigate to the project directory and restore the necessary packages using:
   dotnet restore
4. Set up the database by running the following commands:
   dotnet ef migrations add InitialCreate
   dotnet ef database update
5. Run the application using:
   dotnet run
</details>

<details>
<summary>Usage Instructions 📖</summary>

1. Register by entering your details and uploading a profile picture.
2. Create categories for your transactions.
3. Add transactions specifying the amount, date, note, and type (Income/Expense).
4. View the summary of your finances, latest transactions, and charts displaying your data over time.
</details>

## Demo Video
https://github.com/Omar7001-B/CashGuard/assets/115028809/6d4248a4-5f5d-49bf-886d-381c755a799d



## Contribution 🤝
We welcome contributions from the community. Please fork the repository and submit pull requests for any improvements or bug fixes.
