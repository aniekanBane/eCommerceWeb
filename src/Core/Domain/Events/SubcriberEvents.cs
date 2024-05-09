using eCommerceWeb.Domain.Entities;
using eCommerceWeb.Domain.Primitives.Events;

namespace eCommerceWeb.Domain.Events;

public sealed class SubcribedEvent(Subcriber subcriber) 
    : CreatedEvent<Subcriber>(subcriber);

public sealed class UnSubcribedEvent(Subcriber subcriber) 
    : DeletedEvent<Subcriber>(subcriber);
