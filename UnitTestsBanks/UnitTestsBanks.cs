using Banks.Entities;
using Banks.Entities.BankAccounts;
using Banks.Models;
using Banks.Models.Client;
using Banks.Services;
using Xunit;

namespace UnitTestsBanks;

public class UnitTest1
{
    private ICentralBank _centralBank = CentralBank.GetInstance();
    
    [Fact]
    public void AddBanksWithClients_CentralBankContainsBanksAndClients()
    {
        IClientBuilder builderFirst = new ClientBuilder(12, "Danil", "Doronin");
        IClientBuilder builderSecond = new ClientBuilder(123, "Alex", "Sanchez");
        Bank firstBank = new Bank(0, "Alfa", 15, 50, 1500);
        Bank secondBank = new Bank(1, "Sber", 20, 100, 5000);
        
        _centralBank.AddBank( "Alfa", 15, 50, 1500);
        _centralBank.AddBank("Sber", 20, 100, 5000);
        _centralBank.AddClient(firstBank, 12, "Danil", "Doronin");
        _centralBank.AddClient(secondBank, 123, "Alex", "Sanchez");
        

        Assert.Contains(firstBank, _centralBank.GetBanks());
        Assert.Contains(secondBank, _centralBank.GetBanks());
        Assert.Contains(builderFirst.GetClient(), _centralBank.GetClients(firstBank));
        Assert.Contains(builderSecond.GetClient(), _centralBank.GetClients(secondBank));
    }

    [Fact]
    public void AddSomeAccountsForClients_CheckAccounts()
    {
        IClientBuilder builderFirst = new ClientBuilder(12, "Danil", "Doronin");
        IClientBuilder builderSecond = new ClientBuilder(123, "Alex", "Sanchez");
        Bank firstBank = new Bank(0, "Alfa", 15, 50, 1500);
        Bank secondBank = new Bank(1, "Sber", 20, 100, 5000);
        
        _centralBank.AddBank( "Alfa", 15, 50, 1500);
        _centralBank.AddBank("Sber", 20, 100, 5000);
        _centralBank.AddClient(firstBank, 12, "Danil", "Doronin");
        _centralBank.AddClient(secondBank, 123, "Alex", "Sanchez");
        _centralBank.CreateCreditAccount(firstBank, builderFirst.GetClient(), 123, "alfaCredit", 5000);
        _centralBank.CreateDebutAccount(firstBank, builderFirst.GetClient(), 123, "alfaDebut");
        _centralBank.CreateContribution(firstBank, builderFirst.GetClient(), 123, "alfaContr");
        _centralBank.CreateCreditAccount(secondBank, builderSecond.GetClient(), 123, "sberCredit", 5000);
        _centralBank.CreateDebutAccount(secondBank, builderSecond.GetClient(), 123, "sberDebut");
        _centralBank.CreateContribution(secondBank, builderSecond.GetClient(), 123, "sberContr");
        
        Assert.Equal(3, _centralBank.GetClientAccounts(firstBank, builderFirst.GetClient()).Count);
        Assert.Equal(3, _centralBank.GetClientAccounts(secondBank, builderSecond.GetClient()).Count);
    }
    
    [Fact]
    public void SpecifyClientPassports_CheckPassports()
    {
        IClientBuilder builderFirst = new ClientBuilder(12, "Danil", "Doronin");
        IClientBuilder builderSecond = new ClientBuilder(123, "Alex", "Sanchez");
        Bank firstBank = new Bank(0, "Alfa", 15, 50, 1500);
        Bank secondBank = new Bank(1, "Sber", 20, 100, 5000);
        
        _centralBank.AddBank( "Alfa", 15, 50, 1500);
        _centralBank.AddBank("Sber", 20, 100, 5000);
        _centralBank.AddClient(firstBank, 12, "Danil", "Doronin");
        _centralBank.AddClient(secondBank, 123, "Alex", "Sanchez");
        _centralBank.CreateCreditAccount(firstBank, builderFirst.GetClient(), 123, "alfaCredit", 5000);
        _centralBank.CreateDebutAccount(firstBank, builderFirst.GetClient(), 123, "alfaDebut");
        _centralBank.CreateContribution(firstBank, builderFirst.GetClient(), 123, "alfaContr");
        _centralBank.CreateCreditAccount(secondBank, builderSecond.GetClient(), 123, "sberCredit", 5000);
        _centralBank.CreateDebutAccount(secondBank, builderSecond.GetClient(), 123, "sberDebut");
        _centralBank.CreateContribution(secondBank, builderSecond.GetClient(), 123, "sberContr");
        _centralBank.SpecifyClientPassport(firstBank, builderFirst.GetClient(), new Passport(1234,123456));
        _centralBank.SpecifyClientPassport(secondBank, builderSecond.GetClient(), new Passport(4321, 654321));
        _centralBank.SetTransferLimit(firstBank, 3000);
        
        Assert.Equal(1234, _centralBank.GetClients(firstBank).ToList()[0].Passport.Series);
        Assert.Equal(123456, _centralBank.GetClients(firstBank).ToList()[0].Passport.Number);
        Assert.Equal(4321, _centralBank.GetClients(firstBank).ToList()[1].Passport.Series);
        Assert.Equal(654321, _centralBank.GetClients(firstBank).ToList()[1].Passport.Number);

    }

    [Fact]
    public void MakeSkipDays_CheckAccauntBalances()
    {
        IClientBuilder builderFirst = new ClientBuilder(12, "Danil", "Doronin");
        IClientBuilder builderSecond = new ClientBuilder(123, "Alex", "Sanchez");
        Bank firstBank = new Bank(0, "Alfa", 15, 50, 1500);
        Bank secondBank = new Bank(1, "Sber", 20, 100, 5000);
        
        _centralBank.AddBank( "Alfa", 15, 50, 1500);
        _centralBank.AddBank("Sber", 20, 100, 5000);
        _centralBank.AddClient(firstBank, 12, "Danil", "Doronin");
        _centralBank.AddClient(secondBank, 123, "Alex", "Sanchez");
        _centralBank.CreateCreditAccount(firstBank, builderFirst.GetClient(), 123, "alfaCredit", 5000);
        _centralBank.CreateContribution(secondBank, builderSecond.GetClient(), 123, "sberContr");
        _centralBank.SpecifyClientPassport(firstBank, builderFirst.GetClient(), new Passport(1234,123456));
        _centralBank.SpecifyClientPassport(secondBank, builderSecond.GetClient(), new Passport(4321, 654321));
        _centralBank.SetTransferLimit(firstBank, 3000);
        _centralBank.WithdrawCash(firstBank, _centralBank.GetAccounts(firstBank).ToList()[0], 3000);
        _centralBank.TopUpAccount(secondBank, _centralBank.GetAccounts(firstBank).ToList()[1], 3000);
        _centralBank.PayInterests(40);
        
        Assert.True(_centralBank.GetAccounts(firstBank).ToList()[0].ShowBalance() < 3000);
        Assert.False(_centralBank.GetAccounts(secondBank).ToList()[1].ShowBalance() > 3000);
    }
}