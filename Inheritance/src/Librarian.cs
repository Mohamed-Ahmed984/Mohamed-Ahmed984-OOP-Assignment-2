namespace LibrarySystem;

public class Librarian : Staff
{
    public Librarian(string id, string name, string phone, DateTime hireDate, decimal salary)
        : base(id, name, phone, hireDate, salary, 0m)
    {
        RegisterIdentity();
    }

    public void ProcessReturn(Loan loan, DateTime date)
    {
        if (loan == null)
            throw new ArgumentNullException(nameof(loan), "A loan is required.");
        loan.Return(date);
    }

    public void MarkLost(Loan loan)
    {
        if (loan == null)
            throw new ArgumentNullException(nameof(loan), "A loan is required.");
        loan.MarkLost();
    }
}
