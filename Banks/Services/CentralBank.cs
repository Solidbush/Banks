using Banks.Entities;
using Banks.Entities.BankAccounts;
using Banks.Models;
using Banks.Models.Client;

namespace Banks.Services;

public class CentralBank : ICentralBank
{
    public delegate void InfoMessage(string message);

    public event InfoMessage? Notify;
    
    private static CentralBank _instance;
    private const char Plus = '+';
    private const char Mines = '-';
    private const int MinNumberOfTransaction = 0;
    private const int MinNumberOfBank = 0;
    private readonly List<Bank> _banks;
    private readonly List<Transaction> _transactions;
    private int _tempTransactionNumber;
    private int _tempBankNumber;

    private CentralBank()
    {
        _banks = new List<Bank>();
        _transactions = new List<Transaction>();
        _tempTransactionNumber = MinNumberOfTransaction;
        _tempBankNumber = MinNumberOfBank;
    }

    public static CentralBank GetInstance()
    {
        if (_instance == null)
            _instance = new CentralBank();
        return _instance;
    }

    public void AddBank(string name, decimal interestRate, decimal transferCommission, decimal transferLimit)
    {
        Bank tempBank = new Bank(_tempBankNumber, name, interestRate, transferCommission, transferLimit);
        if (_banks.Contains(tempBank))
            throw new Exception();
        _banks.Add(tempBank);
        _tempBankNumber++;
        Notify += ActivityWithBanks;
        Notify.Invoke($"Bank {_tempBankNumber} {name} added!\n" +
                      $"Interest rate: {interestRate}\n" +
                      $"Transfer commission: {transferCommission}\n" +
                      $"Transfer Limit: {transferLimit}");
        Notify -= ActivityWithBanks;
    }
    public void AddClient(Bank bank, int id, string name, string surname)
    {
        if (!_banks.Contains(bank))
            throw new Exception();
        _banks.FirstOrDefault(bank).AddClient(id, name, surname);
    }

    public void SpecifyClientAddress(Bank bank, Client client, Address address)
    {
        if (!_banks.Contains(bank))
            throw new Exception();
        _banks.FirstOrDefault(bank).SpecifyClientAddress(client, address);
    }

    public void SpecifyClientPassport(Bank bank, Client client, Passport passport)
    {
        if (!_banks.Contains(bank))
            throw new Exception();
        _banks.FirstOrDefault(bank).SpecifyClientPassport(client, passport);
    }

    public void CreateCreditAccount(Bank bank, Client client, int id, string accountName, decimal creditLimit)
    {
        if (!_banks.Contains(bank))
            throw new Exception();
        _banks.FirstOrDefault(bank).CreateCreditAccount(client, id, accountName, creditLimit);
    }

    public void CreateDebutAccount(Bank bank, Client client, int id, string accountName)
    {
        if (!_banks.Contains(bank))
            throw new Exception();
        _banks.FirstOrDefault(bank).CreateDebutAccount(client, id, accountName);
    }

    public void CreateContribution(Bank bank, Client client, int id, string accountName)
    {
        if (!_banks.Contains(bank))
            throw new Exception();
        _banks.FirstOrDefault(bank).CreateContribution(client, id, accountName);
    }

    public void SetInterestRate(Bank bank, decimal newInterestRate)
    {
        if (!_banks.Contains(bank))
            throw new Exception();
        _banks.FirstOrDefault(bank).SetInterestRate(newInterestRate);
    }

    public void SetTransferLimit(Bank bank, decimal newTransferLimit)
    {
        if (!_banks.Contains(bank))
            throw new Exception();
        _banks.FirstOrDefault(bank).SetTransferLimit(newTransferLimit);
    }

    public void SetTransactionCommission(Bank bank, decimal newTransactionCommission)
    {
        if (!_banks.Contains(bank))
            throw new Exception();
        _banks.FirstOrDefault(bank).SetTransactionCommission(newTransactionCommission);
    }

    public void TopUpAccount(Bank bank, IBankAccount account, decimal money)
    {
        if (!_banks.Contains(bank))
            throw new Exception();
        _banks.FirstOrDefault(bank).TopUpAccount(account, money);
    }

    public void WithdrawCash(Bank bank, IBankAccount account, decimal money)
    {
        if (!_banks.Contains(bank))
            throw new Exception();
        _banks.FirstOrDefault(bank).WithdrawCash(account, money);
    }

    public void AddClientInMailingList(Bank bank, Client client)
    {
        if (!_banks.Contains(bank))
            throw new Exception();
        _banks.FirstOrDefault(bank).AddClientInMailingList(client);
    }

    public void RemoveClientFromMailingList(Bank bank, Client client)
    {
        if (!_banks.Contains(bank))
            throw new Exception();
        _banks.FirstOrDefault(bank).RemoveClientFromMailingList(client);
    }

    public IReadOnlyCollection<IBankAccount> GetAccounts(Bank bank)
    {
        if (!_banks.Contains(bank))
            throw new Exception();
        return _banks.FirstOrDefault(bank).GetAccounts();
    }

    public IReadOnlyCollection<Bank> GetBanks()
    {
        return _banks;
    }

    public IReadOnlyCollection<Client> GetClients(Bank bank)
    {
        if (!_banks.Contains(bank))
            throw new Exception();
        return _banks.FirstOrDefault(bank).GetClients();
    }

    public IReadOnlyCollection<Client> GetMailingList(Bank bank)
    {
        if (!_banks.Contains(bank))
            throw new Exception();
        return _banks.FirstOrDefault(bank).GetMailingList();
    }

    public IReadOnlyCollection<IBankAccount> GetClientAccounts(Bank bank, Client client)
    {
        if (!_banks.Contains(bank))
            throw new Exception();
        return _banks.FirstOrDefault(bank).GetClientAccounts(client);
    }

    public void PayInterests(int daysCount)
    {
        foreach (var bank in _banks)
        {
            _banks.FirstOrDefault(bank).PayInterests();
        }
    }

    public decimal ShowAccountBalance(Bank bank, IBankAccount bankAccount)
    {
        if (!_banks.Contains(bank))
            throw new Exception();
        return _banks.FirstOrDefault(bank).ShowAccountBalance(bankAccount);
    }

    public void MakeTransaction(Bank bankSender, Bank bankRecipient, Client sender, IBankAccount accountSender,
        IBankAccount accountRecipient, decimal money)
    {
        if (!_banks.Contains(bankSender))
            throw new Exception();
        if (!_banks.Contains(bankRecipient))
            throw new Exception();
        Transaction transaction = new Transaction(_tempTransactionNumber, bankSender, bankRecipient, sender,
            accountSender, accountRecipient, money, bankSender.GetBankCommission());
        _banks.FirstOrDefault(bankSender).MakeTransfer(accountSender, money);
        _banks.FirstOrDefault(bankRecipient).TopUpAccount(accountRecipient, money);
        _transactions.Add(transaction);
        _tempTransactionNumber++;
        Notify += ActivityWithTransactions;
        Notify.Invoke($"Transaction with id: {transaction.Id} made successful!");
        Notify -= ActivityWithTransactions;
    }

    public void RemoveTransaction(Transaction transaction)
    {
        if (!_transactions.Contains(transaction))
            throw new Exception();
        _banks.FirstOrDefault(transaction.BankSender).RemoveTransaction(Plus, transaction.AccountSender, transaction.Money);
        _banks.FirstOrDefault(transaction.BankRecipient).RemoveTransaction(Mines, transaction.AccountRecipient, transaction.Money);
        _transactions.Remove(transaction);
        Notify += ActivityWithTransactions;
        Notify.Invoke($"Transaction with id: {transaction.Id} removed successful!");
        Notify -= ActivityWithTransactions;
    }

    public IReadOnlyCollection<Transaction> GetTransactions()
    {
        return _transactions;
    }
    
    void ActivityWithBanks(string message)
    { 
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine(message);
        Console.ResetColor();
    }
    
    void ActivityWithTransactions(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}