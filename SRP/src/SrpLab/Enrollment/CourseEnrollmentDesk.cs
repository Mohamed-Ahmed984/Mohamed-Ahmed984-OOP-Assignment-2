namespace SrpLab;

public class CourseEnrollmentDesk
{
    private HashSet<string> _seated = new(StringComparer.OrdinalIgnoreCase);
    private List<string> _waitlist = new();
    public int Capacity { get; }
    public decimal Tuition { get; }
    public string CourseCode { get; }

    public CourseEnrollmentDesk(string courseCode, int capacity, decimal tuition)
    {
        CourseCode = courseCode;
        Capacity = capacity;
        Tuition = tuition;
    }

    public string Register(string studentEmail)
    {
        if (string.IsNullOrWhiteSpace(studentEmail)) throw new ArgumentException("email");
        var email = studentEmail.Trim();

        if (_seated.Contains(email) || _waitlist.Contains(email))
            return "ALREADY_REGISTERED";

        if (_seated.Count < Capacity)
        {
            _seated.Add(email);
            return "SEATED";
        }

        _waitlist.Add(email);
        return $"WAITLIST:{_waitlist.Count}";
    }

    public int WaitlistPosition(string studentEmail)
    {
        for (int i = 0; i < _waitlist.Count; i++)
        {
            if (_waitlist[i].Equals(studentEmail, StringComparison.OrdinalIgnoreCase))
                return i + 1;
        }
        return -1;
    }

    public void PromoteFromWaitlist(int seats)
    {
        while (seats > 0 && _waitlist.Count > 0 && _seated.Count < Capacity)
        {
            var next = _waitlist[0];
            _waitlist.RemoveAt(0);
            _seated.Add(next);
            seats--;
        }
    }

    internal bool HasSeat(string email)
    {
        return _seated.Contains(email);
    }
}
