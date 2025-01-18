using eCommerceWeb.Domain.Primitives.Entities;

namespace eCommerceWeb.Domain.Entities.MarketingAggregate;

public sealed class MailingListSubcriber : IEntity
{
    internal MailingListSubcriber(int subcriberId, int mailingListId)
    {
        SubcriberId = subcriberId;
        MailingListId = mailingListId;
    }
    
    private MailingListSubcriber() { } // EF Core

    public int MailingListId { get; private set; }
    public int SubcriberId { get; private set; }
    public DateTime SubcribedOnUtc { get; private set; }
}
