namespace PatternsLab.Problems.Builder;

public class CourseRegistration
{
    public string StudentEmail { get; }
    public string CourseCode { get; }
    public string AccessMode { get; }
    public string? GroupCode { get; }
    public string? DiscountCode { get; }
    public bool SendWhatsApp { get; }
    public bool SendEmailWelcome { get; }
    public string? MentorNote { get; }
    public DateOnly? PreferredStart { get; }

    private CourseRegistration(Builder builder)
    {
        StudentEmail = builder.StudentEmail;
        CourseCode = builder.CourseCode;
        AccessMode = builder.AccessMode;
        GroupCode = builder.GroupCode;
        DiscountCode = builder.DiscountCode;
        SendWhatsApp = builder.SendWhatsApp;
        SendEmailWelcome = builder.SendEmailWelcome;
        MentorNote = builder.MentorNote;
        PreferredStart = builder.PreferredStart;
    }

    public override string ToString()
    {
        return $"{StudentEmail} → {CourseCode} [{AccessMode}] group={GroupCode ?? "-"} discount={DiscountCode ?? "-"} wa={SendWhatsApp} mail={SendEmailWelcome}";
    }

    public class Builder
    {
        
        internal string StudentEmail { get; }
        internal string CourseCode { get; }
        internal string AccessMode { get; }
        internal string? GroupCode { get; private set; }
        internal string? DiscountCode { get; private set; }
        internal bool SendWhatsApp { get; private set; }
        internal bool SendEmailWelcome { get; private set; }
        internal string? MentorNote { get; private set; }
        internal DateOnly? PreferredStart { get; private set; }

        public Builder(string studentEmail, string courseCode, string accessMode)
        {
            StudentEmail = studentEmail;
            CourseCode = courseCode;
            AccessMode = accessMode;
        }

        public Builder WithGroup(string? value)
        {
            GroupCode = value;
            return this;
        }

        public Builder WithDiscount(string? value)
        {
            DiscountCode = value;
            return this;
        }

        public Builder WithWhatsApp(bool value)
        {
            SendWhatsApp = value;
            return this;
        }

        public Builder WithEmailWelcome(bool value)
        {
            SendEmailWelcome = value;
            return this;
        }

        public Builder WithMentorNote(string? value)
        {
            MentorNote = value;
            return this;
        }

        public Builder WithPreferredStart(DateOnly? value)
        {
            PreferredStart = value;
            return this;
        }

        public CourseRegistration Build()
        {
            if (string.IsNullOrWhiteSpace(StudentEmail))
                throw new ArgumentException("Student email is required.");
            if (string.IsNullOrWhiteSpace(CourseCode))
                throw new ArgumentException("Course code is required.");
            if (AccessMode != "LiveGroup" && AccessMode != "VideosOnly")
                throw new ArgumentException("Access mode must be LiveGroup or VideosOnly.");
            if (AccessMode == "LiveGroup" && string.IsNullOrWhiteSpace(GroupCode))
                throw new InvalidOperationException("LiveGroup requires a GroupCode.");
            if (AccessMode == "VideosOnly" && !string.IsNullOrWhiteSpace(GroupCode))
                throw new InvalidOperationException("VideosOnly cannot have a GroupCode.");

            return new CourseRegistration(this);
        }
    }

}

public static class RegistrationCallSites
{
    public static CourseRegistration CreateLiveStudent()
    {
        return new CourseRegistration.Builder("sara@mail.com", "SEF-101", "LiveGroup")
            .WithGroup("G1")
            .WithDiscount("EARLY10")
            .WithWhatsApp(true)
            .WithEmailWelcome(true)
            .WithMentorNote("Needs evening slot")
            .WithPreferredStart(new DateOnly(2026, 10, 1))
            .Build();
    }

    public static CourseRegistration CreateVideosOnly()
    {
        return new CourseRegistration.Builder("ali@mail.com", "SEF-101", "VideosOnly")
            .WithEmailWelcome(true)
            .Build();
    }
}
