namespace SrpLab;

public class InvoiceNumberSequence
{
    private static int _invoiceSeq = 1000;

    public string NextInvoiceNumber(DateOnly periodStart)
    {
        var n = ++_invoiceSeq;
        return $"INV-{periodStart:yyyyMM}-{n:D5}";
    }
}
