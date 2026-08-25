namespace DPBack.Application.Exceptions;

public class InvalidJsonValuesException : Exception
{
    public InvalidJsonValuesException(string error) : base(error)
    {
    }
}