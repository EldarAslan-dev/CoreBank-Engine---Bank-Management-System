using BankManagementSystem.Models;

namespace BankManagementSystem.Interfaces;

public interface IStorageService
{
    void SaveAccounts(IEnumerable<Account> accounts);
    List<Account> LoadAccounts();
}