using eCommerceWeb.Domain.Primitives.Entities;

namespace eCommerceWeb.Domain.Entities.MarketingAggregate;

public sealed class MailingListSubcriber : AuditableEntity<int>
{
    private MailingListSubcriber() { } // EF Core

    internal MailingListSubcriber(
        int subcriberId, 
        int mailingListId)
    {
        SubcriberId = subcriberId;
        MailingListId = mailingListId;
    }

    public int SubcriberId { get; private set; }
    public int MailingListId { get; private set; }
    public SubcriberStatus SubcriberStatus { get; private set; } = SubcriberStatus.Subcribed();
}
