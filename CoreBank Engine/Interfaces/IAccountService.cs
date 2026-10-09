using BankManagementSystem.Models;

namespace BankManagementSystem.Interfaces;

public interface IAccountService
{
    void CreateAccount(Account account);
    Account GetAccount(string accountNumber);
    IEnumerable<Account> GetAllAccounts();
    void Deposit(string accountNumber, decimal amount);
    void Withdraw(string accountNumber, decimal amount);
    void Transfer(string fromAccountNumber, string toAccountNumber, decimal amount);
    void ApplyInterestToAllSavings();
}