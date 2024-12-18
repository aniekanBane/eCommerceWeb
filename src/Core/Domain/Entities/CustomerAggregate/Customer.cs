using eCommerceWeb.Domain.Primitives.Entities;
using eCommerceWeb.Domain.ValueObjects;

namespace eCommerceWeb.Domain.Entities.CustomerAggregate;

public sealed class Customer : AuditableEntity<Guid>, IAggregateRoot
{
    public Customer(CustomerCreationModel creationModel)
    { 
        Name = new(creationModel.Firstname, creationModel.Lastname);
        EmailAddress = EmailAddress.Of(creationModel.EmailAddress);
        PhoneNumber = PhoneNumber.Of(creationModel.PhoneNumber);
        AcceptsMarketing = creationModel.AcceptsMarketing;
    }

    #pragma warning disable CS8618
    private Customer() { } // EF Core
    #pragma warning restore CS8618

    public Name Name { get; private set; }
    public EmailAddress EmailAddress { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public bool AcceptsMarketing { get; private set; }

    public Customer Update(CustomerUpdateModel updateModel)
    {
        Name = new(updateModel.Firstname, updateModel.Lastname);
        return this;
    }
}
