namespace LibrarySystem;

public class LibraryItem
{
    private static List<string> _catalogNumbers = new List<string>();
    private decimal _lateFeeMultiplier;

    public string CatalogNumber { get; }
    public string Title { get; }
    public int LoanPeriodDays { get; }
    public decimal BaseLateFee { get; private set; }
    public bool IsWithdrawn { get; private set; }
    public bool IsOnLoan { get; private set; }

    protected LibraryItem(string catalog, string title, decimal fee, int days, decimal multiplier)
    {
        if (string.IsNullOrWhiteSpace(catalog))
            throw new ArgumentException("Catalog number is required.");
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.");
        if (fee <= 0)
            throw new ArgumentException("Base late fee must be greater than zero.");
        if (days <= 0 || multiplier <= 0)
            throw new ArgumentException("Loan period and fee multiplier must be positive.");
        if (_catalogNumbers.Contains(catalog.Trim()))
            throw new InvalidOperationException("Catalog number is already registered.");

        CatalogNumber = catalog.Trim();
        Title = title.Trim();
        BaseLateFee = fee;
        LoanPeriodDays = days;
        _lateFeeMultiplier = multiplier;
        _catalogNumbers.Add(CatalogNumber);
    }

    public decimal GetDailyLateFee()
    {
        return BaseLateFee * _lateFeeMultiplier;
    }

    internal void ChangeLateFee(decimal fee)
    {
        if (fee <= 0)
            throw new ArgumentException("New late fee must be greater than zero.");
        BaseLateFee = fee;
    }

    internal void Withdraw()
    {
        IsWithdrawn = true;
    }

    internal void Restore()
    {
        IsWithdrawn = false;
    }

    internal void StartLoan()
    {
        if (IsWithdrawn)
            throw new InvalidOperationException("A withdrawn item cannot be borrowed.");
        if (IsOnLoan)
            throw new InvalidOperationException("This item is already on loan.");
        IsOnLoan = true;
    }

    internal void EndLoan()
    {
        IsOnLoan = false;
    }
}
