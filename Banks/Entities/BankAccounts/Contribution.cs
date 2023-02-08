using System.Text;
using Banks.Models.Client;

namespace Banks.Entities.BankAccounts;

public class Contribution : IBankAccount
{
    private const char Plus = '+';
    private const char Minus = '-';
    private const decimal MinId = 0;
    private const decimal HighBalanceRate = 2;
    private const decimal MaxBalanceRate = 3;
    private const decimal MaxBalance = 100000;
    private const decimal MiddleBalance = 50000;
    private const decimal MinTransferLimit = 0;
    private const decimal MinInterestRate = 0;
    private const int BeginningMonth = 0;
    private const int EndingMonth = 29;
    private const decimal InitialConditions = 0;
    private decimal _transferLimit;
    private decimal _tempTransferSum;
    private decimal _interestRate;
    private decimal _balance;
    private decimal _interest;
    private decimal _tempDays;
    
    public Contribution(int id, Client client, string accountName, decimal transferLimit, decimal interestRate)
    {
        if (string.IsNullOrWhiteSpace(accountName))
            throw new ArgumentNullException(nameof(accountName));
        if (id < MinId)
            throw new Exception();
        if (transferLimit < MinTransferLimit)
            throw new Exception();
        if (interestRate < MinInterestRate)
            throw new Exception();
        Id = id;
        Client = client;
        AccountName = accountName;
        _transferLimit = transferLimit;
        _interestRate = interestRate;
        _tempTransferSum = InitialConditions;
        _balance = InitialConditions;
        _interest = InitialConditions;
        _tempDays = EndingMonth;
    }
    public int Id { get; }
    public Client Client { get; }
    public string AccountName { get; }
    
    public void TopUpAccount(decimal money)
    {
        _balance += Math.Abs(money);
    }

    public void WithdrawCash(decimal money)
    {
        if (_balance - Math.Abs(money) < Decimal.Zero || _tempDays != 0) 
            throw new Exception();
        _balance -= Math.Abs(money);
    }

    public void MakeTransfer(decimal money, decimal commission)
    {
        if (((_balance - Math.Abs(money) - commission) < Decimal.Zero) ||
            (_tempTransferSum + Math.Abs(money) > _transferLimit)) 
            throw new Exception();
        if (_tempDays != 0)
            throw new Exception();
        _balance -= Math.Abs(money) + commission;
        _tempTransferSum += Math.Abs(money);
    }

    public decimal ShowBalance()
    {
        return _balance;
    }

    public void PayInterest()
    {
        if (_tempDays > BeginningMonth)
        {
            if (_balance >= MaxBalance)
            {
                _interest += _balance * _interestRate * MaxBalanceRate;
                return;
            }
            if (_balance >= MiddleBalance)
            {
                _interest += _balance * _interestRate * HighBalanceRate;
                return;
            }
            _interest += _balance * _interestRate;
            _tempDays--;
            return;
        }

        _tempDays = EndingMonth;
        _balance += _interest;
        _tempTransferSum = InitialConditions;
    }

    public void CancellationTransaction(char sign, decimal money, decimal commission)
    {
        if (sign == Plus)
        {
            _balance += Math.Abs(money) + Math.Abs(commission);
            _tempTransferSum -= Math.Abs(money);
            _tempTransferSum = Math.Abs(_tempTransferSum);
            return;
        }
        if (sign == Minus)
        {
            _balance -= Math.Abs(money);
            return;
        }

        throw new Exception();
    }

    public void SetTransferLimit(decimal newTransferLimit)
    {
        if (newTransferLimit < MinTransferLimit)
            throw new Exception();
        _transferLimit = newTransferLimit;
    }

    public void SetInterestRate(decimal newInterestRate)
    {
        if (newInterestRate < MinInterestRate)
            throw new Exception();
        _interestRate = newInterestRate;
    }
    
    protected bool Equals(DebutAccount other)
    {
        return Id == other.Id && Client.Equals(other.Client) && AccountName == other.AccountName;
    }

    public bool Equals(IBankAccount? other)
    {
        return Id == other.Id && Client.Equals(other.Client) && AccountName == other.AccountName;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((DebutAccount)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Client, AccountName);
    }
    
    public override string ToString()
    {
        return new StringBuilder()
            .Append(Id)
            .Append(AccountName)
            .Append(Client)
            .Append(_balance)
            .ToString();
    }
}