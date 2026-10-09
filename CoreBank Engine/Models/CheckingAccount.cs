using BankManagementSystem.Exceptions;
using BankManagementSystem.Models.Enums;

namespace BankManagementSystem.Models;

public class CheckingAccount : Account
{
    public decimal OverdraftLimit { get; set; }

    public CheckingAccount(string accountNumber, string ownerName, decimal initialBalance, decimal overdraftLimit)
        : base(accountNumber, ownerName, initialBalance)
    {
        OverdraftLimit = overdraftLimit;
    }

    public override void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Withdrawal amount must be greater than zero.");

        if (Balance + OverdraftLimit < amount)
            throw new InsufficientFundsException("Transaction declined. Overdraft limit exceeded.");

        Balance -= amount;
        Transactions.Add(new Transaction
        {
            Amount = amount,
            Type = TransactionType.Withdrawal,
            Description = $"Withdrawn from account (Checking): -{amount} AZN"
        });
    }
}