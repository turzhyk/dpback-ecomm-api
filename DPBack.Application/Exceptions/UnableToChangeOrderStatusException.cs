namespace DPBack.Application.Exceptions;

public class UnableToChangeOrderStatusException:Exception
{
    public UnableToChangeOrderStatusException(){}
    public UnableToChangeOrderStatusException(string message) : base(message){}
    public UnableToChangeOrderStatusException(string message, Exception innerException) : base(message, innerException){}
}