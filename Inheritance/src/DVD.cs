namespace LibrarySystem;

public class DVD : LibraryItem
{
    public DVD(string catalog, string title, decimal fee)
        : base(catalog, title, fee, 7, 2m)
    {
    }
}
