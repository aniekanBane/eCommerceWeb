namespace eCommerceWeb.Domain.Primitives.Csv;

public interface ICsvReader<T>
{
    IEnumerable<T> Read(Stream stream);
}
