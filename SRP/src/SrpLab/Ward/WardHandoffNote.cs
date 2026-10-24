namespace SrpLab;

public class WardHandoffNote
{
    private WardBoard _board;

    public WardHandoffNote(WardBoard board)
    {
        _board = board;
    }

    public string BuildHandoffNote(int bed)
    {
        if (!_board.Patients.TryGetValue(bed, out var patient))
            return $"Bed {bed}: empty";

        var acuity = _board.AcuityScores[bed];
        var tone = acuity >= 8 ? "ESCALATE" : acuity >= 4 ? "WATCH" : "STABLE";

        return $"[HANDOFF {DateTime.UtcNow:yyyy-MM-dd}] Bed {bed} · {patient} · acuity={acuity} · {tone}";
    }
}
