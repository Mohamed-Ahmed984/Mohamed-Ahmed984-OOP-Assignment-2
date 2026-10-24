namespace LibrarySystem;

public class Magazine : LibraryItem
{
    public Magazine(string catalog, string title, decimal fee)
        : base(catalog, title, fee, 3, 0.5m)
    {
    }
}
