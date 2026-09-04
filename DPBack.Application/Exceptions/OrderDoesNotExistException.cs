namespace DPBack.Application.Exceptions;

public class OrderDoesNotExistException:Exception
{
    public OrderDoesNotExistException(){}
    public OrderDoesNotExistException(string message, Exception innerException) : base(message, innerException){}
    public OrderDoesNotExistException(Guid id): base($"order {id} does not exist"){}
    public OrderDoesNotExistException(string message) : base(message){}
}