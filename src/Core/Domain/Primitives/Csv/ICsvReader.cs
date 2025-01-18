namespace eCommerceWeb.Domain.Primitives.Csv;

public interface ICsvReader
{
    Task<IEnumerable<T>> ReadAsync<T>(Stream stream);
}
