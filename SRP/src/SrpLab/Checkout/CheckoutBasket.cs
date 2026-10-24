namespace SrpLab;

public class CheckoutBasket
{
    private List<(string Sku, decimal Price, int Qty)> _lines = new();
    private string? _couponRaw;
    private bool _giftWrap;
    internal IReadOnlyList<(string Sku, decimal Price, int Qty)> Lines
    {
        get { return _lines; }
    }

    internal string? CouponText
    {
        get { return _couponRaw; }
    }

    internal bool GiftWrap
    {
        get { return _giftWrap; }
    }

    public void AddLine(string sku, decimal price, int qty)
    {
        if (qty <= 0) throw new ArgumentOutOfRangeException(nameof(qty));
        _lines.Add((sku, price, qty));
    }

    public void ApplyCouponText(string? couponText)
    {
        _couponRaw = couponText;
    }

    public void EnableGiftWrap()
    {
        _giftWrap = true;
    }
}
