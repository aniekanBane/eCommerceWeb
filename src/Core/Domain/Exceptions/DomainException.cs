using System.Globalization;

namespace eCommerceWeb.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException() : base() {}

    public DomainException(string message) : base(message) {}

    public DomainException(string message, Exception innerException) 
        : base(message, innerException) {}

    public DomainException(string message, params object[] args)
        : base(string.Format(CultureInfo.CurrentCulture, message, args)) {}
}
