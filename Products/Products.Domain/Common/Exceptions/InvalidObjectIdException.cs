using System.Diagnostics.CodeAnalysis;

namespace Products.Domain.Common.Exceptions;

[ExcludeFromCodeCoverage]
public class InvalidObjectIdException : Exception
{
    public InvalidObjectIdException(string value)
        : base($"Invalid ObjectId: {value}")
    {
    }
}
