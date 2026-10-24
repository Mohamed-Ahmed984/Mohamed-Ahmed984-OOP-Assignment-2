namespace SrpLab;

public class TuitionInvoice
{
    private CourseEnrollmentDesk _enrollment;

    public TuitionInvoice(CourseEnrollmentDesk enrollment)
    {
        _enrollment = enrollment;
    }

    public string TuitionInvoiceLine(string studentEmail)
    {
        if (!_enrollment.HasSeat(studentEmail)) return $"{_enrollment.CourseCode},WAITLIST,0.00";
        var vat = Math.Round(_enrollment.Tuition * 0.14m, 2);
        return $"{_enrollment.CourseCode},TUITION,{_enrollment.Tuition:0.00},VAT,{vat:0.00},TOTAL,{(_enrollment.Tuition + vat):0.00}";
    }
}
