using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Ardalis.GuardClauses;

public static class GaurdClauseExtensions
{
    public static string InvalidStringLength(
        this IGuardClause gaurdClause, int lenght, [NotNull]string value,
        [CallerArgumentExpression(nameof(value))]string? paramName = null, string? message = null)
    {
        return gaurdClause.LengthOutOfRange(value, lenght, lenght, paramName, message);
    }

    public static void NullOrDefault<T>(
        this IGuardClause gaurdClause, [NotNull][ValidatedNotNull] T? obj, 
        [CallerArgumentExpression(nameof(obj))] string? paramName = null, string? message = null)
    {
        gaurdClause.Null(obj, paramName, message);
        gaurdClause.Default(obj, paramName, message);
    }
}
