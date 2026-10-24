namespace SrpLab;

public class KitchenReadyEstimate
{
    private KitchenTicket _ticket;

    public KitchenReadyEstimate(KitchenTicket ticket)
    {
        _ticket = ticket;
    }

    public int EstimatedReadyMinutes(int openStations)
    {
        if (openStations <= 0)
            openStations = 1;
        int sequential = 0;
        int longest = 0;
        if (_ticket.Items.Count > 0)
            longest = _ticket.Items[0].PrepMinutes;
        foreach (var item in _ticket.Items)
        {
            sequential = checked(sequential + item.PrepMinutes);
            longest = Math.Max(longest, item.PrepMinutes);
        }
        int parallel = (int)Math.Ceiling(sequential / (double)openStations);
        if (new AllergenDetector(_ticket).DetectAllergens().Count > 0)
            parallel += 3;
        return Math.Max(parallel, longest);
    }
}
