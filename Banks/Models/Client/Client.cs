using System.Text;

namespace Banks.Models.Client;

public class Client : IEquatable<Client>
{
    private const int MinClientId = 0;

    public Client(int id, string name, string surname)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name));
        if (string.IsNullOrWhiteSpace(surname))
            throw new ArgumentNullException(nameof(surname));
        if (id < MinClientId)
            throw new Exception();
        Id = id;
        Name = name;
        Surname = surname;
    }
    
    public int Id { get; }
    public string Name { get; }
    public string Surname { get; }
    public Address Address { get; set; }
    public Passport Passport { get; set; }

    public bool Equals(Client? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id == other.Id && Name == other.Name && Surname == other.Surname;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Client)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, Surname);
    }

    public override string ToString()
    {
        return new StringBuilder()
            .Append(Id)
            .Append(Name)
            .Append(Surname)
            .Append(Address)
            .Append(Passport)
            .ToString();
    }
}