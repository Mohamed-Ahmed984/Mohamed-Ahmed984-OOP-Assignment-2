namespace SrpLab;

public class GradePolicy
{
    private GradeBook _gradeBook;

    public GradePolicy(GradeBook gradeBook)
    {
        _gradeBook = gradeBook;
    }

    public string Letter(string studentId)
    {
        var avg = _gradeBook.Average(studentId);
        if (avg >= 90) return "A";
        if (avg >= 80) return "B";
        if (avg >= 70) return "C";
        if (avg >= 60) return "D";
        return "F";
    }

    public bool MeetsHonorRoll(string studentId)
    {
        string letter = Letter(studentId);
        return _gradeBook.Average(studentId) >= 85 && (letter == "A" || letter == "B");
    }
}
