namespace Banks.Exceptions;

public class WrongSeriesException : Exception
{
    public WrongSeriesException()
        : base("Wrong series exception! Check your series input.")
    {
    }

    public WrongSeriesException(string message)
        : base(message)
    {
    }
}