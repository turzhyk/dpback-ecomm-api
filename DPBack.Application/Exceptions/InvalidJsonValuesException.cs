namespace DPBack.Application.Exceptions;

public class InvalidJsonValuesException : Exception
{
    public InvalidJsonValuesException(){}
    public InvalidJsonValuesException(string message) : base(message)
    {
    }
    public InvalidJsonValuesException(string message, Exception innerException) : base(message, innerException)
    {
    }
}