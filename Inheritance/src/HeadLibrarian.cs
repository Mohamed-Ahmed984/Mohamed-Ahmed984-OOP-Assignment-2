namespace LibrarySystem;

public class HeadLibrarian : Staff
{
    public HeadLibrarian(string id, string name, string phone, DateTime hireDate, decimal salary)
        : base(id, name, phone, hireDate, salary, 400m)
    {
        RegisterIdentity();
    }

    public void ChangeLateFee(LibraryItem item, decimal fee)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item), "An item is required.");
        item.ChangeLateFee(fee);
    }

    public void Withdraw(LibraryItem item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item), "An item is required.");
        item.Withdraw();
    }

    public void Restore(LibraryItem item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item), "An item is required.");
        item.Restore();
    }
}
