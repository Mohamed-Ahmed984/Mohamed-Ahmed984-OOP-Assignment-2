namespace LibrarySystem;

public class Loan
{
    public string LoanId { get; }
    public DateTime BorrowDate { get; }
    public Member Member { get; }
    public LibraryItem Item { get; }
    public LoanStatus Status { get; private set; }
    public DateTime? ReturnDate { get; private set; }

    internal Loan(Member member, LibraryItem item, DateTime borrowDate)
    {
        if (member == null)
            throw new ArgumentNullException(nameof(member), "A member is required.");
        if (item == null)
            throw new ArgumentNullException(nameof(item), "An item is required.");
        
        borrowDate.Date.AddDays(item.LoanPeriodDays);

        LoanId = Guid.NewGuid().ToString();
        BorrowDate = borrowDate.Date;
        Member = member;
        Item = item;
        Status = LoanStatus.Borrowed;
        item.StartLoan();
    }

    public DateTime DueDate
    {
        get { return BorrowDate.AddDays(Item.LoanPeriodDays); }
    }

    public decimal LateFee
    {
        get
        {
            if (ReturnDate == null)
                return 0m;
            int lateDays = (ReturnDate.Value - DueDate).Days;
            if (lateDays <= 0)
                return 0m;
            decimal fee = lateDays * Item.GetDailyLateFee();
            return fee * (100m - Member.DiscountPercentage) / 100m;
        }
    }

    internal void Return(DateTime returnDate)
    {
        if (Status != LoanStatus.Borrowed)
            throw new InvalidOperationException("Only a borrowed loan can be returned.");
        if (returnDate.Date < BorrowDate)
            throw new ArgumentException("Return date cannot be earlier than borrow date.");

        ReturnDate = returnDate.Date;
        Status = LoanStatus.Returned;
        Item.EndLoan();
    }

    internal void MarkLost()
    {
        if (Status != LoanStatus.Borrowed)
            throw new InvalidOperationException("Only a borrowed loan can be marked lost.");
        Status = LoanStatus.Lost;
        
    }
}
