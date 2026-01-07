namespace Products.Domain.Common.Exceptions;

public class InvalidObjectIdException : Exception
{
    public InvalidObjectIdException(string value)
        : base($"Invalid ObjectId: {value}")
    {
    }
}
