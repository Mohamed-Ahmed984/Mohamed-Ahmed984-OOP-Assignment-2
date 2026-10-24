namespace SrpLab;

public class GiftCardFormatter
{
    private CheckoutBasket _basket;

    public GiftCardFormatter(CheckoutBasket basket)
    {
        _basket = basket;
    }

    public string GiftMessageCard(string fromName)
    {
        var skus = new List<string>();
        foreach (var line in _basket.Lines)
            skus.Add(line.Sku);
        string items = string.Join(", ", skus);
        decimal total = new BasketPricing(_basket).GrandTotal();
        return $"Dear friend,\nA gift from {fromName} awaits ({items}).\nTotal surprise value: {total:C}\n";
    }
}
