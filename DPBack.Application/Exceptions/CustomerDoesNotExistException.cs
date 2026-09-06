namespace DPBack.Application.Exceptions;

public class CustomerDoesNotExistException:Exception
{
    public CustomerDoesNotExistException(){}
    public CustomerDoesNotExistException(string message) : base(message){}
    public CustomerDoesNotExistException(string message, Exception innerException) : base(message, innerException){}
}