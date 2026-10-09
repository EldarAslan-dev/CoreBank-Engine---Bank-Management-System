using BankManagementSystem.Exceptions;
using BankManagementSystem.Interfaces;
using BankManagementSystem.Models;
using BankManagementSystem.Services;

IStorageService storageService = new JsonStorageService();
IAccountService accountService = new AccountService(storageService);

while (true)
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("\n===========================================");
    Console.WriteLine("        BANK MANAGEMENT SYSTEM (CORE)      ");
    Console.WriteLine("===========================================");
    Console.ResetColor();

    Console.WriteLine("1. Create New Account");
    Console.WriteLine("2. List All Accounts");
    Console.WriteLine("3. Deposit Funds");
    Console.WriteLine("4. Withdraw Funds");
    Console.WriteLine("5. Transfer Funds");
    Console.WriteLine("6. Account Details & Statement");
    Console.WriteLine("7. Apply Interest to Savings Accounts");
    Console.WriteLine("0. Exit");
    Console.Write("\nSelect an option: ");

    string? choice = Console.ReadLine();

    if (choice == "0")
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\nShutting down system. Have a great day!");
        Console.ResetColor();
        break;
    }

    try
    {
        switch (choice)
        {
            case "1":
                CreateAccountMenu(accountService);
                break;
            case "2":
                ListAllAccounts(accountService);
                break;
            case "3":
                DepositMenu(accountService);
                break;
            case "4":
                WithdrawMenu(accountService);
                break;
            case "5":
                TransferMenu(accountService);
                break;
            case "6":
                AccountDetailsMenu(accountService);
                break;
            case "7":
                accountService.ApplyInterestToAllSavings();
                PrintSuccess("Interest rates applied successfully to all savings accounts.");
                break;
            default:
                PrintError("Invalid selection. Please choose an option from the menu.");
                break;
        }
    }
    catch (InsufficientFundsException ex)
    {
        PrintError($"Balance Error: {ex.Message}");
    }
    catch (AccountNotFoundException ex)
    {
        PrintError($"Account Error: {ex.Message}");
    }
    catch (ArgumentException ex)
    {
        PrintError($"Input Error: {ex.Message}");
    }
    catch (Exception ex)
    {
        PrintError($"Unexpected Error: {ex.Message}");
    }
}

static void CreateAccountMenu(IAccountService service)
{
    Console.Write("Account Number: ");
    string accNum = Console.ReadLine()?.Trim() ?? string.Empty;

    Console.Write("Account Holder Name: ");
    string owner = Console.ReadLine()?.Trim() ?? string.Empty;

    Console.Write("Initial Deposit Balance: ");
    decimal balance = decimal.Parse(Console.ReadLine() ?? "0");

    Console.WriteLine("Account Type: 1. Savings Account | 2. Checking Account");
    string typeChoice = Console.ReadLine() ?? "1";

    if (typeChoice == "1")
    {
        Console.Write("Interest Rate (e.g., enter 0.05 for 5%): ");
        decimal rate = decimal.Parse(Console.ReadLine() ?? "0.05");
        service.CreateAccount(new SavingsAccount(accNum, owner, balance, rate));
    }
    else
    {
        Console.Write("Overdraft Credit Limit: ");
        decimal overdraft = decimal.Parse(Console.ReadLine() ?? "0");
        service.CreateAccount(new CheckingAccount(accNum, owner, balance, overdraft));
    }

    PrintSuccess("Account successfully created and saved.");
}

static void ListAllAccounts(IAccountService service)
{
    var accounts = service.GetAllAccounts().ToList();
    if (!accounts.Any())
    {
        Console.WriteLine("\nNo accounts registered.");
        return;
    }

    Console.WriteLine("\n--- ACCOUNT DIRECTORY ---");
    foreach (var acc in accounts)
    {
        string type = acc is SavingsAccount ? "Savings" : "Checking";
        Console.WriteLine($"[{type}] No: {acc.AccountNumber} | Holder: {acc.OwnerName} | Balance: {acc.Balance:N2} AZN");
    }
}

static void DepositMenu(IAccountService service)
{
    Console.Write("Account Number: ");
    string accNum = Console.ReadLine()?.Trim() ?? string.Empty;

    Console.Write("Deposit Amount: ");
    decimal amount = decimal.Parse(Console.ReadLine() ?? "0");

    service.Deposit(accNum, amount);
    PrintSuccess("Funds deposited successfully.");
}

static void WithdrawMenu(IAccountService service)
{
    Console.Write("Account Number: ");
    string accNum = Console.ReadLine()?.Trim() ?? string.Empty;

    Console.Write("Withdrawal Amount: ");
    decimal amount = decimal.Parse(Console.ReadLine() ?? "0");

    service.Withdraw(accNum, amount);
    PrintSuccess("Funds withdrawn successfully.");
}

static void TransferMenu(IAccountService service)
{
    Console.Write("Sender Account Number: ");
    string from = Console.ReadLine()?.Trim() ?? string.Empty;

    Console.Write("Recipient Account Number: ");
    string to = Console.ReadLine()?.Trim() ?? string.Empty;

    Console.Write("Transfer Amount: ");
    decimal amount = decimal.Parse(Console.ReadLine() ?? "0");

    service.Transfer(from, to, amount);
    PrintSuccess("Transfer completed successfully.");
}

static void AccountDetailsMenu(IAccountService service)
{
    Console.Write("Account Number to Inspect: ");
    string accNum = Console.ReadLine()?.Trim() ?? string.Empty;

    var acc = service.GetAccount(accNum);

    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine($"\n--- ACCOUNT SUMMARY [{acc.AccountNumber}] ---");
    Console.WriteLine($"Holder : {acc.OwnerName}");
    Console.WriteLine($"Balance: {acc.Balance:N2} AZN");

    if (acc is SavingsAccount savings)
        Console.WriteLine($"Interest Rate  : %{savings.InterestRate * 100}");
    else if (acc is CheckingAccount checking)
        Console.WriteLine($"Overdraft Limit: {checking.OverdraftLimit:N2} AZN");

    Console.WriteLine("\n--- TRANSACTION HISTORY ---");
    if (!acc.Transactions.Any())
    {
        Console.WriteLine("No transaction history available.");
    }
    else
    {
        foreach (var tx in acc.Transactions)
        {
            Console.WriteLine($"[{tx.Timestamp:yyyy-MM-dd HH:mm}] {tx.Description}");
        }
    }
    Console.ResetColor();
}

static void PrintSuccess(string message)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"\n✔ {message}");
    Console.ResetColor();
}

static void PrintError(string message)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"\n✖ {message}");
    Console.ResetColor();
}