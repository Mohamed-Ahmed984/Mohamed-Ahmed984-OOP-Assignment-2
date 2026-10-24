namespace LibrarySystem;

public class Book : LibraryItem
{
    public Book(string catalog, string title, decimal fee)
        : base(catalog, title, fee, 21, 1m)
    {
    }
}
