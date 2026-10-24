namespace SrpLab;

public class CouponDiscountPolicy
{
    private CheckoutBasket _basket;

    public CouponDiscountPolicy(CheckoutBasket basket)
    {
        _basket = basket;
    }

    public decimal DiscountAmount()
    {
        if (string.IsNullOrWhiteSpace(_basket.CouponText)) return 0m;
        string coupon = _basket.CouponText.Trim().ToUpperInvariant();
        if (coupon.StartsWith("SAVE") && int.TryParse(coupon.Substring(4), out var percentage) && percentage > 0 && percentage <= 50)
            return Math.Round(new BasketPricing(_basket).SubTotal() * percentage / 100m, 2);
        if (coupon.Contains("FREESHIP")) return 0m;
        if (coupon == "WELCOME10") return Math.Min(10m, new BasketPricing(_basket).SubTotal());
        return 0m;
    }
}
