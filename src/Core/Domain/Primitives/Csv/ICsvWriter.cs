namespace eCommerceWeb.Domain.Primitives.Csv;

public interface ICsvWriter
{
    Task WriteAsync<T>(IEnumerable<T> collection, Stream stream);
}
