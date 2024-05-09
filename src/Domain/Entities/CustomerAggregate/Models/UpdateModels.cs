namespace eCommerceWeb.Domain.Entities.CustomerAggregate;

public readonly record struct CustomerUpdateModel(
    string Firstname,
    string Lastname
);
