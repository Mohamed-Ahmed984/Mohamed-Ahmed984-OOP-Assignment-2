namespace SrpLab;

public class WardPager
{
    private List<string> _pagerLog = new List<string>();

    public void Record(int bed, int acuity)
    {
        if (acuity >= 8)
            _pagerLog.Add($"CODE-YELLOW bed={bed} at {DateTime.UtcNow:HH:mm}");
    }

    public IReadOnlyList<string> DrainPagerLog()
    {
        var copy = new List<string>(_pagerLog);
        _pagerLog.Clear();
        return copy;
    }
}
