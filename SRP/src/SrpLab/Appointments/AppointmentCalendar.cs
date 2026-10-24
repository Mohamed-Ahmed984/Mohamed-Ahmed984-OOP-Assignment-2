namespace SrpLab;

public class AppointmentCalendar
{
    private AppointmentDesk _appointments;

    public AppointmentCalendar(AppointmentDesk appointments)
    {
        _appointments = appointments;
    }

    public string ToIcs(DateTimeOffset slot, string patientName, string clinician)
    {
        var uid = Guid.NewGuid();
        var end = slot.AddMinutes(_appointments.SlotMinutes);
        return "BEGIN:VCALENDAR\nVERSION:2.0\nBEGIN:VEVENT\n" +
               $"UID:{uid}\nDTSTART:{slot:yyyyMMdd'T'HHmmss'Z'}\nDTEND:{end:yyyyMMdd'T'HHmmss'Z'}\n" +
               $"SUMMARY:Visit {patientName} / {clinician}\nEND:VEVENT\nEND:VCALENDAR\n";
    }
}
