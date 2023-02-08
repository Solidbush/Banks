using Banks.Entities;
using Banks.Entities.BankAccounts;
using Banks.Models;
using Banks.Models.Client;

namespace Banks.Services;

public interface ICentralBank
{
    void AddBank(string name, decimal interestRate, decimal transferCommission, decimal transferLimit);
    void AddClient(Bank bank, int id, string name, string surname);
    void SpecifyClientAddress(Bank bank, Client client, Address address); 
    void SpecifyClientPassport(Bank bank, Client client, Passport passport);
    void CreateCreditAccount(Bank bank, Client client, int id, string accountName, decimal creditLimit);
    void CreateDebutAccount(Bank bank, Client client, int id, string accountName);
    void CreateContribution(Bank bank, Client client, int id, string accountName);
    void SetInterestRate(Bank bank, decimal newInterestRate);
    void SetTransferLimit(Bank bank, decimal newTransferLimit);
    IReadOnlyCollection<Bank> GetBanks();
    void SetTransactionCommission(Bank bank, decimal newTransactionCommission);
    void TopUpAccount(Bank bank, IBankAccount account, decimal money);
    void WithdrawCash(Bank bank, IBankAccount account, decimal money);
    void AddClientInMailingList(Bank bank, Client client);
    void RemoveClientFromMailingList(Bank bank, Client client);
    IReadOnlyCollection<IBankAccount> GetAccounts(Bank bank);
    IReadOnlyCollection<Client> GetClients(Bank bank);
    IReadOnlyCollection<Client> GetMailingList(Bank bank);
    IReadOnlyCollection<IBankAccount> GetClientAccounts(Bank bank, Client client);
    void PayInterests(int daysCount);
    decimal ShowAccountBalance(Bank bank, IBankAccount bankAccount);

    void MakeTransaction(Bank bankSender, Bank bankRecipient, Client sender, IBankAccount accountSender,
        IBankAccount accountRecipient, decimal money);

    void RemoveTransaction(Transaction transaction);
    IReadOnlyCollection<Transaction> GetTransactions();
}