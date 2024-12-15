using eCommerceWeb.Domain.Primitives.Entities;

namespace eCommerceWeb.Domain.Entities.MarketingAggregate;

public sealed class MailingList: AuditableEntity<int>, IAggregateRoot
{
    #pragma warning disable CS8618
    private MailingList() { } // EF Core
    #pragma warning restore CS8618

    public string Name { get; private set; }
    public string NormalisedName 
    {
        get => Name.ToUpper();
        private set { }
    }

    private readonly List<MailingListSubcriber> _subcribers = [];
    public IReadOnlyCollection<MailingListSubcriber> Subcribers => _subcribers.AsReadOnly();

    public static MailingList Create(string name)
    {
        Guard.Against.NullOrWhiteSpace(name, nameof(name));
        return new MailingList { Name = name };
    }

    public MailingList AddSubcriber(Subcriber subcriber)
    {
        _subcribers.Add(new(subcriber.Id, Id));
        return this;
    }

    public MailingList Rename(string name)
    {
        Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
        return this;
    }
}
