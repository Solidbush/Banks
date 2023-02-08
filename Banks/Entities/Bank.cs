using System.Text;
using Banks.Entities.BankAccounts;
using Banks.Models;
using Banks.Models.Client;

namespace Banks.Entities;

public class Bank : IEquatable<Bank>
{
    private const decimal MinBankId = 0;
    private const decimal MinTransferCommission = 0;
    private const decimal MinInterestRate = 0;
    private const decimal MinTransferLimit = 0;
    private readonly List<IBankAccount> _accounts;
    private readonly List<Client> _clients;
    private readonly List<Client> _mailingList;
    private decimal _interestRate;
    private decimal _transferCommission;
    private decimal _transferLimit;
    
    public delegate void InfoMessage(string message);

    public event InfoMessage? Notify;

    public Bank(int id, string name, decimal interestRate, decimal transferCommission, decimal transferLimit)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name));
        if (id < MinBankId)
            throw new Exception();
        if (interestRate < MinInterestRate)
            throw new Exception();
        if (transferCommission < MinTransferCommission)
            throw new Exception();
        if (transferLimit < MinTransferLimit)
            throw new Exception();
        Id = id;
        Name = name;
        _interestRate = interestRate;
        _transferCommission = transferCommission;
        _accounts = new List<IBankAccount>();
        _clients = new List<Client>();
        _mailingList = new List<Client>();
        _transferLimit = transferLimit;
    }
    public string Name { get; }
    public int Id { get; }

    public void AddClient(int id, string name, string surname)
    {
        IClientBuilder builder = new ClientBuilder(id, name, surname);
        if (_clients.Contains(builder.GetClient()))
            throw new Exception();
        _clients.Add(builder.GetClient());
        Notify += ActivityWithClients;
        Notify.Invoke($"Client {id} {name} {surname} added! Bank: {Id} {Name}");
        Notify -= ActivityWithClients;
    }

    public void SpecifyClientAddress(Client client, Address address)
    {
        if (!_clients.Contains(client))
            throw new Exception();
        IClientBuilder builder = new ClientBuilder(client.Id, client.Name, client.Surname);
        builder.SpecifyClientAddress(address);
        builder.SpecifyClientPassport(client.Passport);
        _clients.Remove(client);
        _clients.Add(builder.GetClient());
        Notify += ActivityWithClients;
        Notify.Invoke($"Client {client.Id} {client.Name} {client.Surname} changed address!\n" +
                      $"New address {address.City} {address.House} {address.Street}!");
        Notify -= ActivityWithClients;
    }

    public void SpecifyClientPassport(Client client, Passport passport)
    {
        if (!_clients.Contains(client))
            throw new Exception();
        IClientBuilder builder = new ClientBuilder(client.Id, client.Name, client.Surname);
        builder.SpecifyClientPassport(passport);
        builder.SpecifyClientAddress(client.Address);
        _clients.Remove(client);
        _clients.Add(builder.GetClient());
        Notify += ActivityWithClients;
        Notify.Invoke($"Client {client.Id} {client.Name} {client.Surname} changed passport!\n" +
                      $"New address {passport.Series} {passport.Number}!");
        Notify -= ActivityWithClients;
    }

    public void CreateCreditAccount(Client client, int id, string accountName, decimal creditLimit)
    {
        IBankAccount newCreditAccount =
            new CreditAccount(id, client, accountName, creditLimit, _transferLimit, _interestRate);
        if (_accounts.Contains(newCreditAccount))
            throw new Exception();
        _accounts.Add(newCreditAccount);
        Notify += ActivityWithAccounts;
        Notify.Invoke($"Client {client.Id} {client.Name} {client.Surname} opened new Credit account!\n" +
                      $"Bank: {Id} {Name}\n" +
                      $"Account: {id} {newCreditAccount} {creditLimit}!\n");
        Notify -= ActivityWithAccounts;
    }

    public void CreateDebutAccount(Client client, int id, string accountName)
    {
        IBankAccount newDebutAccount = new DebutAccount(id, client, accountName, _transferLimit, _interestRate);
        if (_accounts.Contains(newDebutAccount))
            throw new Exception();
        _accounts.Add(newDebutAccount);
        Notify += ActivityWithAccounts;
        Notify.Invoke($"Client {client.Id} {client.Name} {client.Surname} opened new Debut account!\n" +
                      $"Bank: {Id} {Name}\n" +
                      $"Account: {id} {accountName}\n");
        Notify -= ActivityWithAccounts;
    }

    public void CreateContribution(Client client, int id, string accountName)
    {
        IBankAccount newContribution = new Contribution(id, client, accountName, _transferLimit, _interestRate);
        if (_accounts.Contains(newContribution))
            throw new Exception();
        _accounts.Add(newContribution);
        Notify += ActivityWithAccounts;
        Notify.Invoke($"Client {client.Id} {client.Name} {client.Surname} opened new Contribution account!\n" +
                      $"Bank: {Id} {Name}\n" +
                      $"Account: {id} {accountName}\n");
        Notify -= ActivityWithAccounts;
    }

    public void SetInterestRate(decimal newInterestRate)
    {
        if (newInterestRate < MinInterestRate)
            throw new Exception();
        _interestRate = newInterestRate;
        Notify += ActivityWithBanks;
        foreach (var account in _accounts)
        {
            _accounts.FirstOrDefault(account).SetInterestRate(newInterestRate);
            if (_mailingList.Contains(account.Client))
                Notify.Invoke($"{account.Client.Name} {account.Client.Surname}, new Interest Rate for account {account.Id} {account.AccountName}: {newInterestRate}!");
        }

        Notify -= ActivityWithBanks;
    }

    public void SetTransferLimit(decimal newTransferLimit)
    {
        if (newTransferLimit < MinTransferLimit)
            throw new Exception();
        _transferLimit = newTransferLimit;
        Notify += ActivityWithBanks;
        foreach (var account in _accounts)
        {
            _accounts.FirstOrDefault(account).SetTransferLimit(newTransferLimit);
            if (_mailingList.Contains(account.Client))
                Notify.Invoke($"{account.Client.Name} {account.Client.Surname}, new transfer limit for account {account.Id} {account.AccountName}: {newTransferLimit}!");
        }
        Notify -= ActivityWithBanks;
    }

    public void SetTransactionCommission(decimal newTransactionCommission)
    {
        if (newTransactionCommission < MinTransferCommission)
            throw new Exception();
        _transferCommission = newTransactionCommission;
        Notify += ActivityWithBanks;
        foreach (var client in _clients)
        {
            Notify.Invoke($"{client.Name} {client.Surname}, new transaction commission in bank {Id} {Name}: {newTransactionCommission}!");
        }
        Notify -= ActivityWithBanks;
    }

    public void TopUpAccount(IBankAccount account, decimal money)
    {
        if (!_accounts.Contains(account))
            throw new Exception();
        _accounts.FirstOrDefault(account).TopUpAccount(money);
        Notify += ActivityWithAccounts;
        Notify.Invoke($"Account {account.Id} {account.AccountName} topped up! New balance: {_accounts.FirstOrDefault(account).ShowBalance()}");
        Notify -= ActivityWithAccounts;
    }

    public void WithdrawCash(IBankAccount account, decimal money)
    {
        if (!_accounts.Contains(account))
            throw new Exception();
        if (_clients.FirstOrDefault(account.Client).Passport == null)
            throw new Exception();
        _accounts.FirstOrDefault(account).WithdrawCash(money);
        Notify += ActivityWithAccounts;
        Notify.Invoke($"Money {account.Id} {account.AccountName} withdraw success! New balance: {_accounts.FirstOrDefault(account).ShowBalance()}");
        Notify -= ActivityWithAccounts;
    }

    public void MakeTransfer(IBankAccount account, decimal money)
    {
        if (!_accounts.Contains(account))
            throw new Exception();
        if (_clients.FirstOrDefault(account.Client).Passport == null)
            throw new Exception();
        _accounts.FirstOrDefault(account).MakeTransfer(money, _transferCommission);
        Notify += ActivityWithAccounts;
        Notify.Invoke($"Money {account.Id} {account.AccountName} transferred success! New balance: {_accounts.FirstOrDefault(account).ShowBalance()}");
        Notify -= ActivityWithAccounts;
    }

    public void AddClientInMailingList(Client client)
    {
        if (!_clients.Contains(client))
            throw new Exception();
        if (!_mailingList.Contains(client))
            _mailingList.Add(client);
        Notify += ActivityWithClients;
        Notify.Invoke($"Client {client.Id} {client.Name} {client.Surname} added in mailing list!");
        Notify -= ActivityWithClients;
    }

    public void RemoveClientFromMailingList(Client client)
    {
        if (!_clients.Contains(client))
            throw new Exception();
        if (_mailingList.Contains(client))
            _mailingList.Remove(client);
        Notify += ActivityWithClients;
        Notify.Invoke($"Client {client.Id} {client.Name} {client.Surname} removed from mailing list!");
        Notify -= ActivityWithClients;
    }

    public IReadOnlyCollection<IBankAccount> GetAccounts()
    {
        return _accounts;
    }

    public IReadOnlyCollection<Client> GetClients()
    {
        return _clients;
    }

    public IReadOnlyCollection<Client> GetMailingList()
    {
        return _mailingList;
    }

    public void RemoveTransaction(char sign, IBankAccount account, decimal money)
    {
        if (!_accounts.Contains(account))
            throw new Exception();
        _accounts.FirstOrDefault(account).CancellationTransaction(sign, money, _transferCommission);
    }

    public IReadOnlyCollection<IBankAccount> GetClientAccounts(Client client)
    {
        List<IBankAccount> clientAccounts = _accounts.FindAll(e => e.Client == client);
        return clientAccounts;
    }

    public bool CheckClient(Client client)
    {
        return _clients.Contains(client) && client.Passport != null;
    }

    public void PayInterests()
    {
        Notify += PayInterestsInfo;
        foreach (var account in _accounts)
        {
            account.PayInterest();
        }
        Notify.Invoke($"Bank {Id} {Name} paid interests successful!");
        Notify -= PayInterestsInfo;
    }

    public decimal ShowAccountBalance(IBankAccount bankAccount)
    {
        if (!_accounts.Contains(bankAccount))
            throw new Exception();
        return bankAccount.ShowBalance();
    }

    public decimal GetBankCommission()
    {
        return _transferCommission;
    }
    void ActivityWithClients(string message)
    { 
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(message);
        Console.ResetColor();
    }
    
    void ActivityWithAccounts(string message)
    { 
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine(message);
        Console.ResetColor();
    }
    
    void ActivityWithBanks(string message)
    { 
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine(message);
        Console.ResetColor();
    }
    void PayInterestsInfo(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }
    public bool Equals(Bank? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name && Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Bank)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Id);
    }

    public override string ToString()
    {
        return new StringBuilder()
            .Append(Id)
            .Append(Name)
            .ToString();
    }
}