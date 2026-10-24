namespace SrpLab;

public class ClinicHoursPolicy
{
    private AppointmentDesk _appointments;

    public ClinicHoursPolicy(AppointmentDesk appointments)
    {
        _appointments = appointments;
    }

    public bool IsWithinBusinessHours(DateTimeOffset when)
    {
        if (when.DayOfWeek == DayOfWeek.Friday || when.DayOfWeek == DayOfWeek.Saturday) return false;
        var t = TimeOnly.FromDateTime(when.DateTime);
        return t >= _appointments.Open && t.AddMinutes(_appointments.SlotMinutes) <= _appointments.Close;
    }
}
