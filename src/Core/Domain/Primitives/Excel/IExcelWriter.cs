namespace eCommerceWeb.Domain.Primitives.Excel;

/// <summary>
/// Interface for Excel file generation operations.
/// </summary>
public interface IExcelWriter
{
    /// <summary>
    /// Creates an Excel file from a collection of objects.
    /// </summary>
    /// <typeparam name="T">The type of objects in the collection.</typeparam>
    /// <param name="data">The collection to write to Excel.</param>
    /// <param name="sheetName">Optional name for the worksheet.</param>
    /// <returns>The Excel file as a byte array.</returns>
    Task<byte[]> WriteAsync<T>(
        IEnumerable<T> data,
        string sheetName = "Sheet1",
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Creates an Excel file with multiple worksheets.
    /// </summary>
    /// <param name="sheets">Dictionary of sheet names and their data.</param>
    /// <returns>The Excel file as a byte array.</returns>
    Task<byte[]> WriteAsync<T>(
        IDictionary<string, IEnumerable<T>> sheets,
        CancellationToken cancellationToken = default
    );
}
