namespace SrpLab;

public class DunningEmailFormatter
{
    public string Format(string customerName, DateOnly asOf, decimal amount, string invoice, int failures)
    {
        string severity;
        if (failures <= 1)
            severity = "friendly reminder";
        else if (failures == 2)
            severity = "second notice";
        else
            severity = "final notice before suspension";
        return $"Subject: {severity} {invoice}\nHi {customerName},\nBalance {amount:C} as of {asOf:o} ({failures} failures).\n";
    }
}
