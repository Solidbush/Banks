using System.Text;
using Banks.Entities;
using Banks.Entities.BankAccounts;

namespace Banks.Models;

public class Transaction
{
    private const int MinTransactionNumber = 0;
    private const decimal MinBankCommission = 0;
    private const decimal MinMoneyCash = 0;

    public Transaction(int id, Bank bankSender, Bank bankRecipient, Client.Client sender, IBankAccount accountSender,
        IBankAccount accountRecipient, decimal money, decimal bankCommission)
    {
        if (id < MinTransactionNumber)
            throw new Exception();
        if (money < MinMoneyCash)
            throw new Exception();
        if (bankCommission < MinBankCommission)
            throw new Exception();
        Id = id;
        BankSender = bankSender;
        BankRecipient = bankRecipient;
        Sender = sender;
        AccountSender = accountSender;
        AccountRecipient = accountRecipient;
        Money = money;
        BankCommission = bankCommission;
    }
    public int Id { get; }
    public Bank BankSender { get; }
    public Bank BankRecipient { get; }
    public Client.Client Sender { get; }
    public IBankAccount AccountSender { get; }
    public IBankAccount AccountRecipient { get; }
    public decimal Money { get; }
    public decimal BankCommission { get; }

    public override string ToString()
    {
        return new StringBuilder()
            .Append(Id)
            .Append(BankSender)
            .Append(BankRecipient)
            .Append(AccountSender)
            .Append(AccountRecipient)
            .Append(Money)
            .Append(BankCommission)
            .ToString();
    }
    
}