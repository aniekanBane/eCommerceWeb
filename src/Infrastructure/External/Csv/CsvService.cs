using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using eCommerceWeb.Domain.Primitives.Csv;

namespace eCommerceWeb.External.Csv;

internal sealed class CsvService : ICsvReader, ICsvWriter
{
    public Task<IEnumerable<T>> ReadAsync<T>(Stream stream)
    {
        using var reader = new StreamReader(stream);

        var config = new CsvConfiguration(CultureInfo.InvariantCulture) 
        { 
            MissingFieldFound = null 
        };
        using var csv = new CsvReader(reader, config);
        return Task.FromResult(csv.GetRecords<T>());
    }

    public Task WriteAsync<T>(IEnumerable<T> collection, Stream stream)
    {
        using var writer = new StreamWriter(stream);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteRecords(collection);

        return Task.CompletedTask;
    }
}
