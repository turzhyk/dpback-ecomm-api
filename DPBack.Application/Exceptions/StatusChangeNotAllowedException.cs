namespace DPBack.Application.Exceptions;

public class StatusChangeNotAllowedException : Exception
{
    public StatusChangeNotAllowedException()
    {
    }

    public StatusChangeNotAllowedException(string message) : base(message)
    {
    }

    public StatusChangeNotAllowedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}