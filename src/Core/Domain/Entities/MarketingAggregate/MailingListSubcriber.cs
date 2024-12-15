using eCommerceWeb.Domain.Primitives.Entities;

namespace eCommerceWeb.Domain.Entities.MarketingAggregate;

public sealed class MailingListSubcriber : Entity<int>
{
    internal MailingListSubcriber(
        int subcriberId, 
        int mailingListId)
    {
        SubcriberId = subcriberId;
        MailingListId = mailingListId;
    }
    
    private MailingListSubcriber() { } // EF Core

    public int SubcriberId { get; private set; }
    public int MailingListId { get; private set; }
    public SubcriberStatus SubcriberStatus { get; private set; } = SubcriberStatus.Subcribed();
}
