namespace LibrarySystem;

public class StudentMember : Member
{
    public StudentMember(string id, string name, string phone)
        : base(id, name, phone, 3, 0m)
    {
        RegisterIdentity();
    }
}
