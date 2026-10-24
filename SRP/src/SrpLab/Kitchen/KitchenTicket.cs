namespace SrpLab;

public class KitchenTicket
{
    private List<(string Item, List<string> Ingredients, int PrepMinutes)> _items = new();
    internal IReadOnlyList<(string Item, List<string> Ingredients, int PrepMinutes)> Items
    {
        get { return _items; }
    }

    public void AddItem(string item, IEnumerable<string> ingredients, int prepMinutes)
    {
        var normalizedIngredients = new List<string>();
        foreach (string ingredient in ingredients)
            normalizedIngredients.Add(ingredient.Trim().ToLowerInvariant());
        _items.Add((item, normalizedIngredients, prepMinutes));
    }
}
