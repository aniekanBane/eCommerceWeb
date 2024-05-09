namespace eCommerceWeb.Domain.Entities.CustomerAggregate;

public readonly record struct CustomerCreationModel(
    string Firstname,
    string Lastname,
    string EmailAddress,
    string PhoneNumber,
    bool AcceptsMarketing
);
