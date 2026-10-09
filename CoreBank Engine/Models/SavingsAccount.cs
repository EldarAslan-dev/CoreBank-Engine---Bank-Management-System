using BankManagementSystem.Exceptions;
using BankManagementSystem.Models.Enums;

namespace BankManagementSystem.Models;

public class SavingsAccount : Account
{
	public decimal InterestRate { get; set; }

	public SavingsAccount(string accountNumber, string ownerName, decimal initialBalance, decimal interestRate)
		: base(accountNumber, ownerName, initialBalance)
	{
		InterestRate = interestRate;
	}

	public override void Withdraw(decimal amount)
	{
		if (amount <= 0)
			throw new ArgumentException("Withdrawal amount must be greater than zero.");

		if (Balance < amount)
			throw new InsufficientFundsException("Insufficient funds. Savings account balance cannot fall below zero.");

		Balance -= amount;

		Transactions.Add(new Transaction
		{
			Amount = amount,
			Type = TransactionType.Withdrawal,
			Description = $"Withdrawn from account: -{amount} AZN"
		});
	}

	public void ApplyInterest()
	{
		decimal interest = Balance * InterestRate;
		Deposit(interest);
	}
}