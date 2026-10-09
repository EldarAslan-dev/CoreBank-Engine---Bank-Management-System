using BankManagementSystem.Exceptions;
using BankManagementSystem.Interfaces;
using BankManagementSystem.Models;
using BankManagementSystem.Models.Enums;

namespace BankManagementSystem.Services;

public class AccountService : IAccountService
{
    private readonly List<Account> _accounts;
    private readonly IStorageService _storageService;

    public AccountService(IStorageService storageService)
    {
        _storageService = storageService;
        _accounts = _storageService.LoadAccounts();
    }

    public void CreateAccount(Account account)
    {
        if (account == null)
            throw new ArgumentNullException(nameof(account), "Account data cannot be null.");

        if (_accounts.Any(a => a.AccountNumber.Equals(account.AccountNumber, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException($"Account number '{account.AccountNumber}' already exists.");

        _accounts.Add(account);
        _storageService.SaveAccounts(_accounts);
    }

    public Account GetAccount(string accountNumber)
    {
        var account = _accounts.FirstOrDefault(a => a.AccountNumber.Equals(accountNumber, StringComparison.OrdinalIgnoreCase));
        if (account == null)
            throw new AccountNotFoundException($"Account number '{accountNumber}' was not found.");

        return account;
    }

    public IEnumerable<Account> GetAllAccounts()
    {
        return _accounts.AsReadOnly();
    }

    public void Deposit(string accountNumber, decimal amount)
    {
        var account = GetAccount(accountNumber);
        account.Deposit(amount);
        _storageService.SaveAccounts(_accounts);
    }

    public void Withdraw(string accountNumber, decimal amount)
    {
        var account = GetAccount(accountNumber);
        account.Withdraw(amount);
        _storageService.SaveAccounts(_accounts);
    }

    public void Transfer(string fromAccountNumber, string toAccountNumber, decimal amount)
    {
        if (fromAccountNumber.Equals(toAccountNumber, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Cannot transfer funds to the same account.");

        if (amount <= 0)
            throw new ArgumentException("Transfer amount must be greater than zero.");

        var source = GetAccount(fromAccountNumber);
        var target = GetAccount(toAccountNumber);

        source.Withdraw(amount);
        target.Deposit(amount);

        var srcLastTx = source.Transactions.Last();
        source.Transactions.Remove(srcLastTx);
        source.Transactions.Add(new Transaction
        {
            Amount = amount,
            Type = TransactionType.TransferOut,
            Description = $"Transfer to account '{toAccountNumber}': -{amount} AZN"
        });

        var trgLastTx = target.Transactions.Last();
        target.Transactions.Remove(trgLastTx);
        target.Transactions.Add(new Transaction
        {
            Amount = amount,
            Type = TransactionType.TransferIn,
            Description = $"Transfer received from account '{fromAccountNumber}': +{amount} AZN"
        });

        _storageService.SaveAccounts(_accounts);
    }

    public void ApplyInterestToAllSavings()
    {
        var savingsAccounts = _accounts.OfType<SavingsAccount>().ToList();
        foreach (var savings in savingsAccounts)
        {
            savings.ApplyInterest();
        }

        if (savingsAccounts.Any())
        {
            _storageService.SaveAccounts(_accounts);
        }
    }
}