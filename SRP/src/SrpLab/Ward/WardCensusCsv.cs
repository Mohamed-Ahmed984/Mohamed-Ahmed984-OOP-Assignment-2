namespace SrpLab;

public class WardCensusCsv
{
    private WardBoard _board;

    public WardCensusCsv(WardBoard board)
    {
        _board = board;
    }

    public string ExportCensusCsv()
    {
        var lines = new List<string> { "bed,patient,acuity" };
        var beds = new List<int>(_board.Patients.Keys);
        beds.Sort();
        foreach (int bed in beds)
            lines.Add($"{bed},{_board.Patients[bed]},{_board.AcuityScores[bed]}");
        return string.Join('\n', lines);
    }
}
