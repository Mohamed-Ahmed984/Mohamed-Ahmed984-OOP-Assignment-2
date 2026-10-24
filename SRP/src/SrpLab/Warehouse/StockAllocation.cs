namespace SrpLab;

public class StockAllocation
{
    private WarehousePickList _pickList;

    public StockAllocation(WarehousePickList pickList)
    {
        _pickList = pickList;
    }

    public IReadOnlyList<(string Sku, int Allocated)> Allocate()
    {
        var result = new List<(string Sku, int Allocated)>();
        foreach (var line in _pickList.Lines)
        {
            int allocated = AllocatedQuantity(line.QtyNeeded, line.QtyOnHand);
            result.Add((line.Sku, allocated));
        }
        return result;
    }

    public int AllocatedQuantity(int needed, int onHand)
    {
        return Math.Min(needed, onHand);
    }
}
