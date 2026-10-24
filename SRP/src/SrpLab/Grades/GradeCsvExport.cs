namespace SrpLab;

public class GradeCsvExport
{
    private GradeBook _gradeBook;

    public GradeCsvExport(GradeBook gradeBook)
    {
        _gradeBook = gradeBook;
    }

    public string ExportCsv()
    {
        var rows = new List<string> { "studentId,average,letter,honor" };
        var ids = new List<string>(_gradeBook.StudentIds);
        ids.Sort();
        var policy = new GradePolicy(_gradeBook);
        foreach (string id in ids)
        {
            int honor = policy.MeetsHonorRoll(id) ? 1 : 0;
            rows.Add($"{id},{_gradeBook.Average(id)},{policy.Letter(id)},{honor}");
        }
        return string.Join('\n', rows);
    }
}
