namespace SrpLab;

public class GradeBook
{
    private Dictionary<string, List<decimal>> _scores = new(StringComparer.OrdinalIgnoreCase);
    internal IEnumerable<string> StudentIds
    {
        get { return _scores.Keys; }
    }

    public void Record(string studentId, decimal score)
    {
        if (score < 0 || score > 100) throw new ArgumentOutOfRangeException(nameof(score));
        if (!_scores.TryGetValue(studentId, out var list))
        {
            list = new List<decimal>();
            _scores[studentId] = list;
        }
        list.Add(score);
    }

    public decimal Average(string studentId)
    {
        if (!_scores.TryGetValue(studentId, out var scores) || scores.Count == 0)
            return 0m;
        decimal total = 0m;
        foreach (decimal score in scores)
            total += score;
        return Math.Round(total / scores.Count, 2);
    }
}
