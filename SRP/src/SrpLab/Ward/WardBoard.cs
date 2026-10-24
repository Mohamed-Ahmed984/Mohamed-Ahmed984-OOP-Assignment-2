namespace SrpLab;

public class WardBoard
{
    private Dictionary<int, string> _bedPatient = new();
    private Dictionary<int, int> _vitalsScore = new();
    public WardPager Pager { get; } = new WardPager();
    internal IReadOnlyDictionary<int, string> Patients
    {
        get { return _bedPatient; }
    }

    internal IReadOnlyDictionary<int, int> AcuityScores
    {
        get { return _vitalsScore; }
    }

    public void AssignBed(int bed, string patientId, int heartRate, int spo2)
    {
        if (bed <= 0) throw new ArgumentOutOfRangeException(nameof(bed));
        if (string.IsNullOrWhiteSpace(patientId)) throw new ArgumentException("patient required");

        _bedPatient[bed] = patientId.Trim().ToUpperInvariant();
        _vitalsScore[bed] = new AcuityScorer().ScoreAcuity(heartRate, spo2);

        Pager.Record(bed, _vitalsScore[bed]);
    }
}
