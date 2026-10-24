namespace LibrarySystem;

public class PremiumMember : Member
{
    public PremiumMember(string id, string name, string phone, decimal discount)
        : base(id, name, phone, 10, discount)
    {
        RegisterIdentity();
    }

    public int ReadingPoints
    {
        get
        {
            int returnedLoans = 0;
            foreach (Loan loan in Loans)
            {
                if (loan.Status == LoanStatus.Returned)
                    returnedLoans++;
            }
            return returnedLoans * 5;
        }
    }
}
