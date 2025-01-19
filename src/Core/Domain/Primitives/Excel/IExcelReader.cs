namespace eCommerceWeb.Domain.Primitives.Excel;

/// <summary>
/// Interface for Excel file reading operations.
/// </summary>
public interface IExcelReader
{
    /// <summary>
    /// Reads data from an Excel file and maps it to a collection of objects.
    /// </summary>
    /// <typeparam name="T">The type to map the Excel data to.</typeparam>
    /// <param name="stream">The Excel file stream.</param>
    /// <param name="sheetName">Optional sheet name to read from (defaults to first sheet).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of mapped objects.</returns>
    Task<IEnumerable<T>> ReadAsync<T>(
        Stream stream,
        string? sheetName = default,
        CancellationToken cancellationToken = default
    );
}
