namespace SrpLab;

public class LoanDecisionLetter
{
    private LoanDesk _application;

    public LoanDecisionLetter(LoanDesk application)
    {
        _application = application;
    }

    public string DecisionLetter(string applicantName)
    {
        if (new LoanEligibility(_application).IsEligible())
        {
            return $"Dear {applicantName},\nYour request for {_application.RequestedAmount:C} is pre-approved (risk {new LoanEligibility(_application).RiskScore():0}).\n" +
                   $"Please upload: {string.Join("; ", new LoanDocumentRequirements(_application).RequiredDocuments())}.\n";
        }

        return $"Dear {applicantName},\nWe are unable to approve {_application.RequestedAmount:C} at this time.\n" +
               $"Reference risk={new LoanEligibility(_application).RiskScore():0}. You may reapply after improving documentation.\n";
    }
}
