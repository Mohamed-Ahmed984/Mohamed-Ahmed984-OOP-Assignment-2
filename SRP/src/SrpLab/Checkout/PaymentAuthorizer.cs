namespace SrpLab;

public class PaymentAuthorizer
{
    private CheckoutBasket _basket;

    public PaymentAuthorizer(CheckoutBasket basket)
    {
        _basket = basket;
    }

    public string AuthorizePaymentStub(string cardLast4)
    {
        var payload = $"{new BasketPricing(_basket).GrandTotal():0.00}|{cardLast4}|{_basket.Lines.Count}";
        var hash = payload.GetHashCode();
        return $"AUTH-{Math.Abs(hash):X8}";
    }
}
