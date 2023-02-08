namespace Banks.Exceptions;

public class WrongNumberException : Exception
{
    public WrongNumberException()
        : base("Wrong number exception! Check your number input.")
    {
    }

    public WrongNumberException(string message)
        : base(message)
    {
    }
}