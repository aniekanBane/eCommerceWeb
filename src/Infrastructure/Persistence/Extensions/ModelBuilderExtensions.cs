using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace eCommerceWeb.Persistence.Extensions;

internal static class ModelBuilderExtensions
{
    public static void ApplyGlobalFilter<TType>(
        this ModelBuilder modelBuilder, 
        Expression<Func<TType, bool>> filter)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes()
            .Where(e => e.ClrType.IsAssignableTo(typeof(TType))))
        {
            var param = Expression.Parameter(entity.ClrType);
            var body = ReplacingExpressionVisitor.Replace(filter.Parameters.Single(), param, filter.Body);
            var expression = Expression.Lambda(body, param);

            entity.SetQueryFilter(expression);
        }
    }
}
