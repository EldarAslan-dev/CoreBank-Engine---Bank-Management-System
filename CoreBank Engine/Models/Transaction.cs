using BankManagementSystem.Models.Enums;

namespace BankManagementSystem.Models;

public class Transaction
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
    public decimal Amount { get; init; }
    public TransactionType Type { get; init; }
    public string Description { get; init; } = string.Empty;
}