using Banks.Models.Client;

namespace Banks.Entities.BankAccounts;

public interface IBankAccount : IEquatable<IBankAccount>
{
    public int Id { get; }
    public Client Client { get; }
    
    public string AccountName { get; }

    void TopUpAccount(decimal money);
    void WithdrawCash(decimal money);
    void MakeTransfer(decimal money, decimal commission);
    decimal ShowBalance();
    void PayInterest();
    void CancellationTransaction(char sign, decimal money, decimal commission);
    void SetTransferLimit(decimal newTransferLimit);
    void SetInterestRate(decimal newInterestRate);
}