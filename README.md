
# OOPBankManagementSystem

# 🏦 OOP Bank Management System

A modern **Bank Management System** built using **ASP.NET Core MVC**, **Entity Framework Core**, **SQL Server**,  and **Object-Oriented Programming (OOP)** principles.

This project was developed as a portfolio project to demonstrate enterprise-level architecture, CRUD operations, real-time notifications, layered design, and clean coding practices.

---

# Features

## Dashboard
- Live statistics
- Customer count
- Account count
- Total balance
- Recent transactions
- Quick actions

## Customer Management
- Add Customer
- View Customers
- Active Status
- Database Integration

## Account Management
- Create Savings / Current Account
- Customer Mapping
- Initial Balance
- Account Status

## Fund Transfer
- Transfer money between accounts
- Balance validation
- Automatic balance update
- Transfer history generation

## Transaction Management
- Deposit
- Withdrawal
- Transfer In
- Transfer Out
- Transaction History

## Loan Management
- Apply Loan
- Interest Calculation
- Monthly Installment Calculation
- Loan Status
- Loan Categories

## Card Management
- Issue Debit Card
- Issue Credit Card
- Card Limit
- Card Status
- Expiry Date

## Reports
- Financial Summary
- Customer Report
- Transaction Report
- Account Report

## Settings
- Bank Information
- Security Settings
- Transaction Rules

## Real-Time Notifications
- SignalR Integration
- Live Notification System

---

# 🛠 Technologies Used

- ASP.NET Core MVC
- C#
- Entity Framework Core
- SQL Server
- LINQ
- Bootstrap 5
- HTML5
- CSS3
- JavaScript
- SignalR
- Dependency Injection

---

# 📂 Project Architecture

```
Controllers
│
├── HomeController

Services
│
├── IFrontendService
└── FrontendService

Models
│
├── Customer
├── Account
├── Transaction
├── Loan
├── Card
└── OOP
      ├── BankAccount
      ├── SavingsAccount
      └── CurrentAccount

Data
│
└── AppDbContext

Hubs
│
└── NotificationHub

Views
│
└── Home
```

---

# 💻 OOP Concepts Used

## ✅ Encapsulation

Balance is kept private inside the BankAccount class and accessed only through methods.

```
private decimal Balance;

public decimal GetBalance()
{
    return Balance;
}
```

---

## ✅ Inheritance

SavingsAccount and CurrentAccount inherit from BankAccount.

```
BankAccount
    │
    ├── SavingsAccount
    └── CurrentAccount
```

---

## ✅ Abstraction

BankAccount defines common banking operations while child classes provide specific implementations.

```
public abstract class BankAccount
{
    public abstract bool Withdraw(decimal amount);
}
```

---

## ✅ Polymorphism

The project uses polymorphism during money transfer.

```
BankAccount account;

if(type=="Savings")
    account = new SavingsAccount();

else
    account = new CurrentAccount();

account.Transfer(...);
```

Same method call executes different implementations depending on the object type.

---

# 📊 Database

SQL Server Database

Tables

- Customers
- Accounts
- Transactions
- Loans
- Cards
- Users

---

# ✨ Key Features

- Clean Architecture
- Layered Design
- Dependency Injection
- Entity Framework Core
- LINQ Queries
- CRUD Operations
- Modal Forms
- Real-Time Notifications
- Responsive Dashboard

---

# 📸 Screenshots

## Dashboard

<img width="1536" height="1024" alt="Dashboard" src="https://github.com/user-attachments/assets/3ecc39d8-3224-4cfb-99a8-9e44123bcf6a" />


# ⚙️ Installation

Clone the repository

```
git clone https://github.com/YourUsername/OOPBankManagementSystem.git
```

Navigate to project

```
cd OOPBankManagementSystem
```

Restore packages

```
dotnet restore
```

Update Connection String

```
appsettings.json
```

Run Project

```
dotnet run
```

---

# 📈 Future Improvements
- Email Notifications
- PDF Reports
- Account Statements
- Interest Automation
- ATM Simulation
- Mobile Banking API

---

Developer

Muqtadir Shinwari

