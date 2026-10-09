using System.Text.Json;
using BankManagementSystem.Interfaces;
using BankManagementSystem.Models;

namespace BankManagementSystem.Services;

public class JsonStorageService : IStorageService
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _options;

    private class AccountDto
    {
        public string AccountType { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public string OwnerName { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public decimal SpecialValue { get; set; }
        public List<Transaction> Transactions { get; set; } = new();
    }

    public JsonStorageService(string filePath = "bank_data.json")
    {
        _filePath = filePath;
        _options = new JsonSerializerOptions { WriteIndented = true };
    }

    public void SaveAccounts(IEnumerable<Account> accounts)
    {
        var dtoList = new List<AccountDto>();

        foreach (var acc in accounts)
        {
            var dto = new AccountDto
            {
                AccountNumber = acc.AccountNumber,
                OwnerName = acc.OwnerName,
                Balance = acc.Balance,
                Transactions = acc.Transactions
            };

            if (acc is SavingsAccount savings)
            {
                dto.AccountType = nameof(SavingsAccount);
                dto.SpecialValue = savings.InterestRate;
            }
            else if (acc is CheckingAccount checking)
            {
                dto.AccountType = nameof(CheckingAccount);
                dto.SpecialValue = checking.OverdraftLimit;
            }

            dtoList.Add(dto);
        }

        string json = JsonSerializer.Serialize(dtoList, _options);
        File.WriteAllText(_filePath, json);
    }

    public List<Account> LoadAccounts()
    {
        if (!File.Exists(_filePath))
            return new List<Account>();

        string json = File.ReadAllText(_filePath);
        if (string.IsNullOrWhiteSpace(json))
            return new List<Account>();

        var dtoList = JsonSerializer.Deserialize<List<AccountDto>>(json, _options);
        var accounts = new List<Account>();

        if (dtoList == null)
            return accounts;

        foreach (var dto in dtoList)
        {
            Account acc;
            if (dto.AccountType == nameof(SavingsAccount))
            {
                acc = new SavingsAccount(dto.AccountNumber, dto.OwnerName, dto.Balance, dto.SpecialValue);
            }
            else
            {
                acc = new CheckingAccount(dto.AccountNumber, dto.OwnerName, dto.Balance, dto.SpecialValue);
            }

            acc.Transactions.Clear();
            acc.Transactions.AddRange(dto.Transactions);
            accounts.Add(acc);
        }

        return accounts;
    }
}