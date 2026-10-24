namespace SrpLab;

public class SubscriptionDocuments
{
    private SubscriptionBilling _subscription;

    public SubscriptionDocuments(SubscriptionBilling subscription)
    {
        _subscription = subscription;
    }

    public string DunningEmail(string customerName, DateOnly asOf)
    {
        var amount = new SubscriptionProration(_subscription).Prorate(_subscription.PeriodStart);
        var invoice = new InvoiceNumberSequence().NextInvoiceNumber(_subscription.PeriodStart);
        return new DunningEmailFormatter().Format(customerName, asOf, amount, invoice, _subscription.FailedPayments);
    }

    public string LedgerJournalLine(DateOnly activeFrom)
    {
        string invoice = new InvoiceNumberSequence().NextInvoiceNumber(_subscription.PeriodStart);
        decimal amount = new SubscriptionProration(_subscription).Prorate(activeFrom);
        return new SubscriptionLedgerCsv().Format(_subscription.CustomerId, invoice, amount);
    }
}
