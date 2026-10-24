namespace SrpLab;

public class CourseWelcomePacket
{
    private CourseEnrollmentDesk _enrollment;

    public CourseWelcomePacket(CourseEnrollmentDesk enrollment)
    {
        _enrollment = enrollment;
    }

    public string WelcomePacketMarkdown(string studentEmail, string studentName)
    {
        var status = _enrollment.HasSeat(studentEmail) ? "confirmed seat" : $"waitlist #{_enrollment.WaitlistPosition(studentEmail)}";
        return $"# Welcome to {_enrollment.CourseCode}\nHi {studentName},\nYour status: **{status}**.\n" +
               $"Bring a laptop. Discord onboarding link: https://example.invalid/{_enrollment.CourseCode.ToLowerInvariant()}\n";
    }
}
