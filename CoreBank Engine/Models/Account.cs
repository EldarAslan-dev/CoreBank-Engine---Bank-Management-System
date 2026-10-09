using BankManagementSystem.Models.Enums;

namespace BankManagementSystem.Models;

public abstract class Account
{
    public string AccountNumber { get; init; }
    public string OwnerName { get; set; }
    public decimal Balance { get; protected set; }
    public List<Transaction> Transactions { get; init; } = new();

    protected Account(string accountNumber, string ownerName, decimal initialBalance)
    {
        AccountNumber = accountNumber;
        OwnerName = ownerName;
        Balance = initialBalance;
    }

    public virtual void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Deposit amount must be greater than zero.");

        Balance += amount;
        Transactions.Add(new Transaction
        {
            Amount = amount,
            Type = TransactionType.Deposit,
            Description = $"Deposited to account: +{amount} AZN"
        });
    }

    public abstract void Withdraw(decimal amount);
}