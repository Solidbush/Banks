namespace Banks.Models.Client;

public class ClientBuilder : IClientBuilder
{
    private readonly Client _client;
    public ClientBuilder(int id, string name, string surname)
    {
        _client = new Client(id, name, surname);
    }
    public IClientBuilder SpecifyClientAddress(Address address)
    {
        _client.Address = address;
        return this;
    }

    public IClientBuilder SpecifyClientPassport(Passport passport)
    {
        _client.Passport = passport;
        return this;
    }

    public Client GetClient()
    {
        return _client;
    }
}