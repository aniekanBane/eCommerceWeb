using System.Data;
using ClosedXML.Excel;
using eCommerceWeb.Domain.Primitives.Excel;

namespace eCommerceWeb.External.Excel;

internal sealed class ClosedXMLService : IExcelWriter, IExcelReader
{
    private const string author = "E-CommerceWeb"; // TODO: Should come from app setting app name

    #region Writer Implementation
    public async Task<byte[]> WriteAsync<T>(
        IEnumerable<T> data,
        string sheetName = "Sheet1",
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(data);

        using var workbook = CreateWorkbook();

        var worksheet = AddWorksheet(workbook, sheetName);
        worksheet.Cell(1, 1).InsertTable(data);

        return await SaveWorkbookAsync(workbook, cancellationToken);
    }

    public async Task<byte[]> WriteAsync<T>(
        IDictionary<string, IEnumerable<T>> sheets,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(sheets);
        
        if (!sheets.Any())
        {
            throw new ArgumentException("At least one sheet is required", nameof(sheets));
        }

        using var workbook = CreateWorkbook();
        foreach (var (sheetName, data) in sheets)
        {
            var worksheet = AddWorksheet(workbook, sheetName);
            worksheet.Cell(1, 1).InsertTable(data);
        }

        return await SaveWorkbookAsync(workbook, cancellationToken);
    }
    #endregion

    #region Reader Implementation
    public async Task<IEnumerable<T>> ReadAsync<T>(
        Stream stream,
        string? sheetName = default,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return await Task.Run(() =>
        {
            using var workbook = new XLWorkbook(stream);
            
            var worksheet = (string.IsNullOrWhiteSpace(sheetName)
                ? workbook.Worksheets.FirstOrDefault()
                : workbook.TryGetWorksheet(sheetName, out var ws) ? ws : null) ?? throw new InvalidOperationException($"Worksheet {sheetName ?? "default"} not found");
            
            return worksheet.RangeUsed()
                ?.AsTable()
                .AsNativeDataTable()
                .AsEnumerable()
                .Select(MapRowToObject<T>)
                .ToList() ?? [];

        }, cancellationToken);
    }
    #endregion

    #region Private Helpers
    private static XLWorkbook CreateWorkbook()
    {
        var workbook = new XLWorkbook();
        workbook.Properties.Author = author;
        return workbook;
    }

    private static IXLWorksheet AddWorksheet(XLWorkbook workbook, string name)
    {
        var worksheet = workbook.AddWorksheet(name);
        worksheet.Cells().Style
            .Font.SetFontSize(12)
            .Font.SetFontName("Calibri");
        return worksheet;
    }

    private static async Task<byte[]> SaveWorkbookAsync(
        XLWorkbook workbook,
        CancellationToken cancellationToken)
    {
        using var memoryStream = new MemoryStream();
        await Task.Run(() => workbook.SaveAs(memoryStream), cancellationToken);
        return memoryStream.ToArray();
    }

    private static T MapRowToObject<T>(DataRow row)
    {
        var obj = Activator.CreateInstance<T>();
        var properties = typeof(T).GetProperties();

        foreach (var prop in properties)
        {
            if (!row.Table.Columns.Contains(prop.Name))
                continue;

            var value = row[prop.Name];
            if (value != DBNull.Value)
            {
                var convertedValue = Convert.ChangeType(value, prop.PropertyType);
                prop.SetValue(obj, convertedValue);
            }
        }

        return obj;
    }
    #endregion
}
