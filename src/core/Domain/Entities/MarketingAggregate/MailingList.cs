using eCommerceWeb.Domain.Primitives.Entities;

namespace eCommerceWeb.Domain.Entities.MarketingAggregate;

public sealed class MailingList: AuditableEntity<int>, IAggregateRoot
{
    private MailingList(string name) => Name = name;

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
        return new MailingList(name);
    }

    public MailingList AddSubcriber(Subcriber subcriber)
    {
        _subcribers.Add(new (subcriber.Id, Id));
        return this;
    }

    public MailingList Rename(string name)
    {
        Guard.Against.NullOrWhiteSpace(name, nameof(name));

        Name = name;
        
        return this;
    }
}
