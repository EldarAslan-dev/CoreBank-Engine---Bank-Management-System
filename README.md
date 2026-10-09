# 🏦 CoreBank Engine - Bank Management System

![.NET Core](https://img.shields.io/badge/.NET%20Core-10.0-purple?style=flat&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-14.0-blue?style=flat&logo=csharp)
![Architecture](https://img.shields.io/badge/Architecture-Modular%20N--Tier-brightgreen)
![Persistence](https://img.shields.io/badge/Persistence-JSON%20File%20Storage-orange)

A modular, enterprise-architected Bank Management System built in C# .NET to demonstrate foundational Clean Code, SOLID principles, custom domain exceptions, polymorphism, and asynchronous-ready file persistence.

---

## 🌟 Key Features

- **Object-Oriented Architecture:**
  - `Account` base abstraction with custom rules for derived account models.
  - `SavingsAccount`: Non-negative balance constraint with automated interest calculation.
  - `CheckingAccount`: Dynamic overdraft protection allowing negative balances up to a configured threshold.
- **Audit & Transaction Logging:**
  - Automated ledger tracking every deposit, withdrawal, and peer-to-peer transfer with immutable unique identifiers (`Guid`) and UTC timestamps.
- **Domain Exception Handling:**
  - Custom domain exceptions (`InsufficientFundsException`, `AccountNotFoundException`) preventing invalid states without runtime crashes.
- **Persistence Layer:**
  - Decoupled storage using `IStorageService` and `JsonStorageService`, implementing Data Transfer Objects (DTO) to handle polymorphic JSON serialization.
- **CLI Interactive Dashboard:**
  - User-friendly terminal interface featuring color-coded status badges, real-time validations, and statement generators.

---

## 📂 Project Architecture

The solution follows a strict separation of concerns:

```text
BankManagementSystem/
│
├── Exceptions/               # Domain-specific runtime exceptions
│   ├── AccountNotFoundException.cs
│   └── InsufficientFundsException.cs
│
├── Interfaces/               # Service contracts & abstraction layer (SOLID / DIP)
│   ├── IAccountService.cs
│   └── IStorageService.cs
│
├── Models/                   # Core business entities & domain logic
│   ├── Enums/
│   │   └── TransactionType.cs
│   ├── Account.cs            # Abstract base account
│   ├── CheckingAccount.cs    # Overdraft enabled account
│   ├── SavingsAccount.cs     # Interest bearing account
│   └── Transaction.cs        # Transaction record
│
├── Services/                 # Business logic implementation & persistence
│   ├── AccountService.cs     # Core banking ledger management
│   └── JsonStorageService.cs # File-based data storage engine
│
└── Program.cs                # Console presentation layer & entry point
