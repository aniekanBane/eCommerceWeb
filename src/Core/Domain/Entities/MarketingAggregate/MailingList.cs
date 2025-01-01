using eCommerceWeb.Domain.Primitives.Entities;

namespace eCommerceWeb.Domain.Entities.MarketingAggregate;

public sealed class MailingList: AuditableEntity<int>, IAggregateRoot
{
    public MailingList(string name)
    {
        Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
    }

    #pragma warning disable CS8618
    private MailingList() { } // EF Core
    #pragma warning restore CS8618

    public string Name { get; private set; }
    public string NormalizedName 
    {
        get => Name.ToUpper();
        private set { }
    }

    private readonly List<MailingListSubcriber> _subcribers = [];
    public IReadOnlyCollection<MailingListSubcriber> Subcribers => _subcribers.AsReadOnly();

    public void AddSubcriber(int subcriberId)
    {
        _subcribers.Add(new(subcriberId, Id));
    }

    public MailingList Rename(string name)
    {
        Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
        return this;
    }
}
