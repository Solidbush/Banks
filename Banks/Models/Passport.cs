using System.Text;

namespace Banks.Models;

public class Passport : IEquatable<Passport>
{
    private const int LowerSeriesBound = 1000;
    private const int UpperSeriesLimit = 9999;
    private const int LowerNumberBound = 100000;
    private const int UpperNumberLimit = 999999;

    public Passport(int series, int number)
    {
        if (series is > UpperSeriesLimit or < LowerSeriesBound)
            throw new Exception();
        if (number is > UpperNumberLimit or < LowerNumberBound)
            throw new Exception();
        Series = series;
        Number = number;
    }
    
    public int Series { get; }
    public int Number { get; }

    public bool Equals(Passport? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Series == other.Series && Number == other.Number;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Passport)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Series, Number);
    }

    public override string ToString()
    {
        return new StringBuilder()
            .Append(Series)
            .Append(Number)
            .ToString();
    }
}