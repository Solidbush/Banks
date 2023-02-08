using System.Text;

namespace Banks.Models;

public class Address
{
    public Address(string city, string street, string house)
    {
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentNullException(nameof(city));
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentNullException(nameof(street));
        if (string.IsNullOrWhiteSpace(house))
            throw new ArgumentNullException(nameof(house));
        City = city;
        Street = street;
        House = house;
    }
    public string Street { get; }
    public string City { get; }
    public string House { get; }
    
    public override string ToString()
    {
        return new StringBuilder()
            .Append(City)
            .Append(Street)
            .Append(House)
            .ToString();
    }
}