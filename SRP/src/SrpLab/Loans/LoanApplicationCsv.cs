namespace SrpLab;

public class LoanApplicationCsv
{
    private LoanDesk _application;

    public LoanApplicationCsv(LoanDesk application)
    {
        _application = application;
    }

    public string UnderwriterCsvRow(string applicationId)
    {
        return $"{applicationId},{_application.CreditScore},{_application.EmploymentMonths},{(_application.HasCollateral ? 1 : 0)},{new LoanEligibility(_application).RiskScore():0.00},{(new LoanEligibility(_application).IsEligible() ? "Y" : "N")}";
    }
}
