namespace SrpLab;

public class LoanEligibility
{
    private LoanDesk _application;

    public LoanEligibility(LoanDesk application)
    {
        _application = application;
    }

    public decimal RiskScore()
    {
        decimal score = 100m;
        score -= Math.Max(0, 700 - _application.CreditScore) * 0.15m;
        if (_application.EmploymentMonths < 6) score -= 20m;
        if (_application.RequestedAmount > 50_000m && !_application.HasCollateral) score -= 25m;
        if (_application.RequestedAmount > 150_000m) score -= 10m;
        return Math.Clamp(score, 0m, 100m);
    }

    public bool IsEligible()
    {
        return RiskScore() >= 55m && _application.CreditScore >= 580;
    }
}
