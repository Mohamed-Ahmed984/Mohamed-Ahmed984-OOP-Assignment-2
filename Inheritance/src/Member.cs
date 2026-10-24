namespace LibrarySystem;

public class Member : Person
{
    private List<Loan> _loans = new List<Loan>();

    public int MaxLoans { get; }
    public decimal DiscountPercentage { get; }
    public IReadOnlyList<Loan> Loans
    {
        get { return _loans.AsReadOnly(); }
    }

    protected Member(string id, string name, string phone, int maxLoans, decimal discount)
        : base(id, name, phone)
    {
        if (maxLoans <= 0)
            throw new ArgumentException("Loan limit must be positive.");
        if (discount < 0 || discount > 100)
            throw new ArgumentException("Discount must be between 0 and 100 percent.");

        MaxLoans = maxLoans;
        DiscountPercentage = discount;
    }

    public Loan Borrow(LibraryItem item, DateTime date)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item), "An item is required.");

        int activeLoans = 0;
        foreach (Loan loan in _loans)
        {
            if (loan.Status == LoanStatus.Borrowed)
                activeLoans++;
        }

        if (activeLoans >= MaxLoans)
            throw new InvalidOperationException("Member has reached the active loan limit.");

        Loan newLoan = new Loan(this, item, date);
        _loans.Add(newLoan);
        return newLoan;
    }
}
