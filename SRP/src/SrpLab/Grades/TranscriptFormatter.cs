namespace SrpLab;

public class TranscriptFormatter
{
    private GradeBook _gradeBook;

    public TranscriptFormatter(GradeBook gradeBook)
    {
        _gradeBook = gradeBook;
    }

    public string TranscriptPlain(string studentId, string fullName)
    {
        return $"TRANSCRIPT\nStudent: {fullName} ({studentId})\nAverage: {_gradeBook.Average(studentId)}\nLetter: {new GradePolicy(_gradeBook).Letter(studentId)}\nHonor: {new GradePolicy(_gradeBook).MeetsHonorRoll(studentId)}\n";
    }
}
