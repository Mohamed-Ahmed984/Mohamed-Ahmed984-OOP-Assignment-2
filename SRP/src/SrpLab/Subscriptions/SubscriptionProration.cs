namespace SrpLab;

public class SubscriptionProration
{
    private SubscriptionBilling _subscription;

    public SubscriptionProration(SubscriptionBilling subscription)
    {
        _subscription = subscription;
    }

    public decimal Prorate(DateOnly activeFrom)
    {
        if (activeFrom <= _subscription.PeriodStart) return _subscription.MonthlyPrice;
        if (activeFrom >= _subscription.PeriodEnd) return 0m;
        var totalDays = _subscription.PeriodEnd.DayNumber - _subscription.PeriodStart.DayNumber;
        if (totalDays <= 0) return _subscription.MonthlyPrice;
        var used = _subscription.PeriodEnd.DayNumber - activeFrom.DayNumber;
        return Math.Round(_subscription.MonthlyPrice * used / totalDays, 2);
    }
}
