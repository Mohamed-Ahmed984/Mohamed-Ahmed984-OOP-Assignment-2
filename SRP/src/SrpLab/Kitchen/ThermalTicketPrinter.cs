namespace SrpLab;

public class ThermalTicketPrinter
{
    private KitchenTicket _ticket;

    public ThermalTicketPrinter(KitchenTicket ticket)
    {
        _ticket = ticket;
    }

    public string RenderThermalTicket(int orderNumber)
    {
        string line = new string('=', 32);
        var itemLines = new List<string>();
        foreach (var item in _ticket.Items)
            itemLines.Add($"* {item.Item.ToUpperInvariant()} ({item.PrepMinutes}m)");
        string body = string.Join('\n', itemLines);
        var allergens = new AllergenDetector(_ticket).DetectAllergens();
        string allergyLine = "ALLERGENS: none";
        if (allergens.Count > 0)
            allergyLine = "ALLERGENS: " + string.Join(",", allergens);
        int minutes = new KitchenReadyEstimate(_ticket).EstimatedReadyMinutes(2);
        return $"{line}\nORDER #{orderNumber}\nETA {minutes} MIN\n{body}\n{allergyLine}\n{line}\n";
    }
}
