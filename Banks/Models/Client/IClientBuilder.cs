namespace Banks.Models.Client;

public interface IClientBuilder
{
    IClientBuilder SpecifyClientAddress(Address address);
    IClientBuilder SpecifyClientPassport(Passport passport);
    Client GetClient();
}