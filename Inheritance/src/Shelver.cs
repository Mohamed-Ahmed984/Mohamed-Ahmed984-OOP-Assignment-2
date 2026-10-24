namespace LibrarySystem;

public class Shelver : Staff
{
    public string Section { get; private set; }

    public Shelver(string id, string name, string phone, DateTime hireDate,
        decimal salary, string section) : base(id, name, phone, hireDate, salary, 0m)
    {
        if (string.IsNullOrWhiteSpace(section))
            throw new ArgumentException("Section is required.");
        Section = section.Trim();
        RegisterIdentity();
    }

    public void Reassign(string section)
    {
        if (string.IsNullOrWhiteSpace(section))
            throw new ArgumentException("Section is required.");
        Section = section.Trim();
    }
}
