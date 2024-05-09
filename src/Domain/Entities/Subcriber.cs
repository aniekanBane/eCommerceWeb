using eCommerceWeb.Domain.Entities.MarketingAggregate;
using eCommerceWeb.Domain.Events;
using eCommerceWeb.Domain.Primitives.Entities;
using eCommerceWeb.Domain.ValueObjects;

namespace eCommerceWeb.Domain.Entities;

public sealed class Subcriber : AuditableEntityWithDomainEvent<int>, IAggregateRoot
{
    #pragma warning disable CS8618
    private Subcriber() { } // EF Core

    public Subcriber(SubcriberCreationModel creationModel)
    {
        Name = new(creationModel.Firstname, creationModel.Lastname);
        EmailAddress = EmailAddress.Of(creationModel.EmailAddress);
        AcceptsMarketing = creationModel.AcceptsMarketing;

        RaiseDomainEvent(new SubcribedEvent(this));
    }

    public Name Name { get; private set; }
    public EmailAddress EmailAddress { get; init; }
    public bool AcceptsMarketing { get; private set; }

    private readonly List<MailingListSubcriber> _mailingLists = [];
    public IReadOnlyCollection<MailingListSubcriber> MailingLists => _mailingLists.AsReadOnly();

    public void UnSubcribe() => RaiseDomainEvent(new UnSubcribedEvent(this));

    public Subcriber Update(SubcriberUpdateModel updateModel)
    {
        Name = new(updateModel.Firstname, updateModel.Lastname);
        return this;
    }
}

public readonly record struct SubcriberCreationModel(
    string Firstname,
    string Lastname,
    string EmailAddress,
    bool AcceptsMarketing
);

public readonly record struct SubcriberUpdateModel(
    string Firstname,
    string Lastname
);
