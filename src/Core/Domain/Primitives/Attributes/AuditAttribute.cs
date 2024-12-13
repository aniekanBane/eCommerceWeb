namespace eCommerceWeb.Domain.Primitives.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class AuditableAttribute : Attribute;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class NotAuditableAttribute : Attribute;
