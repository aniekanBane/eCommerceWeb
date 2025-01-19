using eCommerceWeb.Domain.Primitives.SysTime;

namespace eCommerceWeb.External.SysDateTime;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
    public DateTimeOffset OffsetNow => DateTimeOffset.UtcNow;
    public DateTimeOffset OffsetUtcNow => DateTimeOffset.UtcNow;
}
