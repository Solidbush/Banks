using Banks.Entities;
using Banks.Models;
using Banks.Models.Client;
using Banks.Services;

ICentralBank centralBank = CentralBank.GetInstance();

decimal GetDecimal()
{
    return Convert.ToDecimal(Console.ReadLine());
}

int GetInt()
{
    return Convert.ToInt32(Console.ReadLine());
}

void PrintList<T>(List<T> array)
{
    int index = 0;
    foreach (var item in array)
    {
        Console.WriteLine($"{index}. {item.ToString()}");
        index++;
    }
}


void AddBank()
{
    Console.WriteLine("Input Bank's name: ");
    string name = Console.ReadLine();
    Console.WriteLine("Input interest Rate for bank: ");
    decimal interestRate = GetDecimal();
    Console.WriteLine("Input transfer commission for bank: ");
    decimal transferCommission = GetDecimal();
    Console.WriteLine("Input transfer limit for bank: ");
    decimal transferLimit = GetDecimal();
    centralBank.AddBank(name, interestRate, transferCommission, transferLimit);
    
}

void AddClient()
{
    Console.WriteLine("Chose bank for add client: ");
    PrintList(centralBank.GetBanks().ToList());
    int index = GetInt();
    Console.WriteLine("Input Client's id: ");
    int id = GetInt();
    Console.WriteLine("Input client's name: ");
    string name = Console.ReadLine();
    Console.WriteLine("Input client's surname: ");
    string surname = Console.ReadLine();
    centralBank.AddClient(centralBank.GetBanks().ToList()[index], id, name, surname);
}

void SpecifyClientAddress()
{
    Console.WriteLine("Chose bank: ");
    PrintList(centralBank.GetBanks().ToList());
    int index = GetInt();
    PrintList(centralBank.GetBanks().ToList()[index].GetClients().ToList());
    Console.WriteLine("Chose client: ");
    int clientIndex = GetInt();
    Console.WriteLine("Input client's city: ");
    string city = Console.ReadLine();
    Console.WriteLine("Input client's street: ");
    string street = Console.ReadLine();
    Console.WriteLine("Input client's house: ");
    string house = Console.ReadLine();
    centralBank.SpecifyClientAddress(centralBank.GetBanks().ToList()[index],
        centralBank.GetBanks().ToList()[index].GetClients().ToList()[clientIndex], new Address(city, street, house));
}

void SpecifyClientPassport()
{
    Console.WriteLine("Chose bank: ");
    PrintList(centralBank.GetBanks().ToList());
    int index = GetInt();
    Console.WriteLine("Chose client: ");
    PrintList(centralBank.GetBanks().ToList()[index].GetClients().ToList());
    int clientIndex = GetInt();
    Console.WriteLine("Input client's passport series: ");
    int series = GetInt();
    Console.WriteLine("Input client's passport number: ");
    int number = GetInt();
    centralBank.SpecifyClientPassport(centralBank.GetBanks().ToList()[index],
        centralBank.GetBanks().ToList()[index].GetClients().ToList()[clientIndex], new Passport(series, number));
    
}

void CreateCreditAccount()
{
    Console.WriteLine("Chose bank: ");
    PrintList(centralBank.GetBanks().ToList());
    int index = GetInt();
    PrintList(centralBank.GetBanks().ToList()[index].GetClients().ToList());
    Console.WriteLine("Chose client: ");
    int clientIndex = GetInt();
    Console.WriteLine("Input account's id: ");
    int id = GetInt();
    Console.WriteLine("Input account's name: ");
    string name = Console.ReadLine();
    Console.WriteLine("Input account's credit limit: ");
    decimal creditLimit = GetDecimal();
    centralBank.CreateCreditAccount(centralBank.GetBanks().ToList()[index],
        centralBank.GetBanks().ToList()[index].GetClients().ToList()[clientIndex], id, name, creditLimit);
}

void CreateDebutAccount()
{
    Console.WriteLine("Chose bank: ");
    PrintList(centralBank.GetBanks().ToList());
    int index = GetInt();
    PrintList(centralBank.GetBanks().ToList()[index].GetClients().ToList());
    Console.WriteLine("Chose client: ");
    int clientIndex = GetInt();
    Console.WriteLine("Input account's id: ");
    int id = GetInt();
    Console.WriteLine("Input account's name: ");
    string name = Console.ReadLine();
    centralBank.CreateDebutAccount(centralBank.GetBanks().ToList()[index],
        centralBank.GetBanks().ToList()[index].GetClients().ToList()[clientIndex], id, name);
}

void CreateContribution()
{
    Console.WriteLine("Chose bank: ");
    PrintList(centralBank.GetBanks().ToList());
    int index = GetInt();
    PrintList(centralBank.GetBanks().ToList()[index].GetClients().ToList());
    Console.WriteLine("Chose client: ");
    int clientIndex = GetInt();
    Console.WriteLine("Input account's id: ");
    int id = GetInt();
    Console.WriteLine("Input account's name: ");
    string name = Console.ReadLine();
    centralBank.CreateContribution(centralBank.GetBanks().ToList()[index],
        centralBank.GetBanks().ToList()[index].GetClients().ToList()[clientIndex], id, name);
}

void SetInterestRate()
{
    Console.WriteLine("Chose bank: ");
    PrintList(centralBank.GetBanks().ToList());
    int index = GetInt();
    Console.WriteLine("Input new interest rate: ");
    decimal interestRate = GetDecimal();
    centralBank.SetInterestRate(centralBank.GetBanks().ToList()[index], interestRate);
}

void SetTransferLimit()
{
    Console.WriteLine("Chose bank: ");
    PrintList(centralBank.GetBanks().ToList());
    int index = GetInt();
    Console.WriteLine("Input new transfer limit: ");
    decimal transferLimit = GetDecimal();
    centralBank.SetTransferLimit(centralBank.GetBanks().ToList()[index], transferLimit);
}

void ShowBanks()
{
    PrintList(centralBank.GetBanks().ToList());
}

void SetTransactionCommission()
{
    Console.WriteLine("Chose bank: ");
    PrintList(centralBank.GetBanks().ToList());
    int index = GetInt();
    Console.WriteLine("Input new transaction commission: ");
    decimal transactionCommission = GetDecimal();
    centralBank.SetTransactionCommission(centralBank.GetBanks().ToList()[index], transactionCommission);
}

void TopUpAccount()
{
    Console.WriteLine("Chose bank: ");
    PrintList(centralBank.GetBanks().ToList());
    int index = GetInt();
    Console.WriteLine("Chose client: ");
    PrintList(centralBank.GetBanks().ToList()[index].GetClients().ToList());
    int clientIndex = GetInt();
    PrintList(centralBank.GetClientAccounts(centralBank.GetBanks().ToList()[index],
        centralBank.GetBanks().ToList()[index].GetClients().ToList()[clientIndex]).ToList());
    Console.WriteLine("Chose account: ");
    int accountId = GetInt();
    Console.WriteLine("Input count of money: ");
    decimal money = GetDecimal();
    centralBank.TopUpAccount(centralBank.GetBanks().ToList()[index],
        centralBank.GetClientAccounts(centralBank.GetBanks().ToList()[index],
            centralBank.GetBanks().ToList()[index].GetClients().ToList()[clientIndex]).ToList()[accountId], money);
}

void WithdrawCash()
{
    Console.WriteLine("Chose bank: ");
    PrintList(centralBank.GetBanks().ToList());
    int index = GetInt();
    Console.WriteLine("Chose client: ");
    PrintList(centralBank.GetBanks().ToList()[index].GetClients().ToList());
    int clientIndex = GetInt();
    PrintList(centralBank.GetClientAccounts(centralBank.GetBanks().ToList()[index],
        centralBank.GetBanks().ToList()[index].GetClients().ToList()[clientIndex]).ToList());
    Console.WriteLine("Chose account: ");
    int accountId = GetInt();
    Console.WriteLine("Input count of money for withdraw: ");
    decimal money = GetDecimal();
    centralBank.WithdrawCash(centralBank.GetBanks().ToList()[index],
        centralBank.GetClientAccounts(centralBank.GetBanks().ToList()[index],
            centralBank.GetBanks().ToList()[index].GetClients().ToList()[clientIndex]).ToList()[accountId], money);
}

void AddClientInMailingList()
{
    Console.WriteLine("Chose bank: ");
    PrintList(centralBank.GetBanks().ToList());
    int index = GetInt();
    Console.WriteLine("Chose client: ");
    PrintList(centralBank.GetBanks().ToList()[index].GetClients().ToList());
    int clientIndex = GetInt();
    centralBank.AddClientInMailingList(centralBank.GetBanks().ToList()[index],
        centralBank.GetClients(centralBank.GetBanks().ToList()[index]).ToList()[clientIndex]);
}

void RemoveClientFromMailingList()
{
    Console.WriteLine("Chose bank: ");
    PrintList(centralBank.GetBanks().ToList());
    int index = GetInt();
    Console.WriteLine("Chose client: ");
    PrintList(centralBank.GetBanks().ToList()[index].GetClients().ToList());
    int clientIndex = GetInt();
    centralBank.RemoveClientFromMailingList(centralBank.GetBanks().ToList()[index],
        centralBank.GetClients(centralBank.GetBanks().ToList()[index]).ToList()[clientIndex]);
}

void GetAccounts()
{
    Console.WriteLine("Chose bank: ");
    PrintList(centralBank.GetBanks().ToList());
    int index = GetInt();
    PrintList(centralBank.GetAccounts(centralBank.GetBanks().ToList()[index]).ToList());
}

void GetClients()
{
    Console.WriteLine("Chose bank: ");
    PrintList(centralBank.GetBanks().ToList());
    int index = GetInt();
    PrintList(centralBank.GetClients(centralBank.GetBanks().ToList()[index]).ToList());
}

void GetMailingList()
{
    Console.WriteLine("Chose bank: ");
    PrintList(centralBank.GetBanks().ToList());
    int index = GetInt();
    PrintList(centralBank.GetMailingList(centralBank.GetBanks().ToList()[index]).ToList());
}

void GetClientAccounts()
{
    Console.WriteLine("Chose bank: ");
    PrintList(centralBank.GetBanks().ToList());
    int index = GetInt();
    Console.WriteLine("Chose client: ");
    PrintList(centralBank.GetBanks().ToList()[index].GetClients().ToList());
    int clientIndex = GetInt();
    PrintList(centralBank.GetClientAccounts(centralBank.GetBanks().ToList()[index],
        centralBank.GetBanks().ToList()[index].GetClients().ToList()[clientIndex]).ToList());
}

void PayInterests()
{
    Console.WriteLine("What count of days do you want to skip?: ");
    int days = GetInt();
    centralBank.PayInterests(days);
}

void ShowAccountBalance()
{
    Console.WriteLine("Chose bank: ");
    PrintList(centralBank.GetBanks().ToList());
    int index = GetInt();
    Console.WriteLine("Chose client: ");
    PrintList(centralBank.GetBanks().ToList()[index].GetClients().ToList());
    int clientIndex = GetInt();
    Console.WriteLine("Chose account: ");
    PrintList(centralBank.GetClientAccounts(centralBank.GetBanks().ToList()[index],
        centralBank.GetBanks().ToList()[index].GetClients().ToList()[clientIndex]).ToList());
    int accountId = GetInt();
    centralBank.ShowAccountBalance(centralBank.GetBanks().ToList()[index], centralBank.GetClientAccounts(centralBank.GetBanks().ToList()[index],
        centralBank.GetBanks().ToList()[index].GetClients().ToList()[clientIndex]).ToList()[accountId]);
}

void MakeTransaction()
{
    Console.WriteLine("Chose bank sender: ");
    PrintList(centralBank.GetBanks().ToList());
    int bankSender = GetInt();
    Console.WriteLine("Chose bank recipient: ");
    PrintList(centralBank.GetBanks().ToList());
    int bankRecipient = GetInt();
    Console.WriteLine("Chose client sender: ");
    PrintList(centralBank.GetBanks().ToList()[bankSender].GetClients().ToList());
    int clientIndex = GetInt();
    Console.WriteLine("Chose account: ");
    PrintList(centralBank.GetClientAccounts(centralBank.GetBanks().ToList()[bankSender],
        centralBank.GetBanks().ToList()[bankSender].GetClients().ToList()[clientIndex]).ToList());
    int accountSender = GetInt();
    Console.WriteLine("Chose account: ");
    PrintList(centralBank.GetClientAccounts(centralBank.GetBanks().ToList()[bankRecipient],
        centralBank.GetBanks().ToList()[bankRecipient].GetClients().ToList()[clientIndex]).ToList());
    int acauntRecipient = GetInt();
    Console.WriteLine("Input count of money for transaction: ");
    decimal money = GetDecimal();
    centralBank.MakeTransaction(centralBank.GetBanks().ToList()[bankSender],
        centralBank.GetBanks().ToList()[bankRecipient],
        centralBank.GetBanks().ToList()[bankSender].GetClients().ToList()[clientIndex],
        centralBank.GetClientAccounts(centralBank.GetBanks().ToList()[bankSender],
            centralBank.GetBanks().ToList()[bankSender].GetClients().ToList()[clientIndex]).ToList()[accountSender],
        centralBank.GetAccounts(centralBank.GetBanks().ToList()[bankRecipient]).ToList()[acauntRecipient], money);
}

void RemoveTransaction()
{
    Console.WriteLine("Chose transaction's number: ");
    PrintList(centralBank.GetTransactions().ToList());
    int index = GetInt();
    centralBank.RemoveTransaction(centralBank.GetTransactions().ToList()[index]);
}

void GetTransactions()
{
    PrintList(centralBank.GetTransactions().ToList());
}

void ShowCommands()
{
    Console.WriteLine("1. For Add New Bank");
    Console.WriteLine("2. For Add New Client");
    Console.WriteLine("3. Specify Client Address");
    Console.WriteLine("4. Specify Client Passport");
    Console.WriteLine("5. Create Credit Account");
    Console.WriteLine("6. Create Debut Account");
    Console.WriteLine("7. Create Contribution");
    Console.WriteLine("8. Set Interest Rate");
    Console.WriteLine("9. Set Transfer Limit");
    Console.WriteLine("10. Show Banks");
    Console.WriteLine("11. Set Transaction Commission");
    Console.WriteLine("12. Top Up Account");
    Console.WriteLine("13. Withdraw Cash From Account");
    Console.WriteLine("14. Add Client In Mailing List");
    Console.WriteLine("15. Remove Client From Mailing List");
    Console.WriteLine("16. Get Accounts In Bank");
    Console.WriteLine("17. Get Clients In Bank");
    Console.WriteLine("18. Get Mailing List In Bank");
    Console.WriteLine("19. Get Client Accounts In Bank");
    Console.WriteLine("20. Skip Days For Pay Interests");
    Console.WriteLine("21. Show Account Balance");
    Console.WriteLine("22. Make Transaction Between Accounts");
    Console.WriteLine("23. Remove Transaction Between Accounts");
    Console.WriteLine("24. Get All Transactions");
    Console.WriteLine("25. ShowCommands");
}

void CheckCommand()
{
    int command = GetInt();
    switch (command)
    {
        case 1: AddBank();
            break;
        case 2: AddClient();
            break;
        case 3: SpecifyClientAddress();
            break;
        case 4: SpecifyClientPassport();
            break;
        case 5: CreateCreditAccount();
            break;
        case 6: CreateDebutAccount();
            break;
        case 7: CreateContribution();
            break;
        case 8: SetInterestRate();
            break;
        case 9: SetTransferLimit();
            break;
        case 10: ShowBanks();
            break;
        case 11: SetTransactionCommission();
            break;
        case 12: TopUpAccount();
            break;
        case 13: WithdrawCash();
            break;
        case 14: AddClientInMailingList();
            break;
        case 15: RemoveClientFromMailingList();
            break;
        case 16: GetAccounts();
            break;
        case 17: GetClients();
            break;
        case 18: GetMailingList();
            break;
        case 19: GetClientAccounts();
            break;
        case 20: PayInterests();
            break;
        case 21: ShowAccountBalance();
            break;
        case 22: MakeTransaction();
            break;
        case 23: RemoveTransaction();
            break;
        case 24: GetTransactions();
            break;
        case 25: ShowCommands();
            break;
        default: ShowCommands();
            break;
    }
    ShowCommands();
}

while (true)
{
    ShowCommands();
    CheckCommand();
}