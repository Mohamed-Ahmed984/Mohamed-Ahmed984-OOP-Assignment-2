namespace SrpLab;

public class BasketPricing
{
    private CheckoutBasket _basket;

    public BasketPricing(CheckoutBasket basket)
    {
        _basket = basket;
    }

    public decimal SubTotal()
    {
        decimal total = 0m;
        foreach (var line in _basket.Lines)
            total += line.Price * line.Qty;
        return total;
    }

    public decimal GrandTotal()
    {
        var total = SubTotal() - new CouponDiscountPolicy(_basket).DiscountAmount();
        if (_basket.GiftWrap)
            total += 4.99m;
        return Math.Max(0m, total);
    }
}
