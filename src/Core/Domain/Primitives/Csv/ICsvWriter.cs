namespace eCommerceWeb.Domain.Primitives.Csv;

public interface ICsvWriter<T>
{
    void Write(IEnumerable<T> collection, Stream stream);
}
