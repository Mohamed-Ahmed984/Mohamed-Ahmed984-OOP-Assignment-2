namespace SrpLab;

public class LoanDocumentRequirements
{
    private LoanDesk _application;

    public LoanDocumentRequirements(LoanDesk application)
    {
        _application = application;
    }

    public IReadOnlyList<string> RequiredDocuments()
    {
        var docs = new List<string> { "National ID", "Proof of income (3 months)" };
        if (_application.RequestedAmount > 40_000m) docs.Add("Bank statements (6 months)");
        if (_application.HasCollateral) docs.Add("Collateral ownership deed");
        if (_application.EmploymentMonths < 12) docs.Add("Employer letter");
        if (!new LoanEligibility(_application).IsEligible()) docs.Add("Manual underwriter referral form");
        return docs;
    }
}
