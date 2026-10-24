namespace LibrarySystem;

public class Person
{
    private static List<string> _personIds = new List<string>();

    public string PersonId { get; }
    public string FullName { get; }
    public string Phone { get; }

    protected Person(string id, string fullName, string phone)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Person ID is required.");
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name is required.");
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone is required.");

        PersonId = id.Trim();
        FullName = fullName.Trim();
        Phone = phone.Trim();
    }

    protected void RegisterIdentity()
    {
        
        if (_personIds.Contains(PersonId))
            throw new InvalidOperationException("Person ID is already registered.");
        _personIds.Add(PersonId);
    }
}
