namespace SrpLab;

public class PickWalkingOrder
{
    private WarehousePickList _pickList;

    public PickWalkingOrder(WarehousePickList pickList)
    {
        _pickList = pickList;
    }

    public IReadOnlyList<(string Aisle, int Bin, string Sku, int Qty)> WalkingOrder()
    {
        var result = new List<(string Aisle, int Bin, string Sku, int Qty)>();
        var allocation = new StockAllocation(_pickList);
        foreach (var line in _pickList.Lines)
        {
            int quantity = allocation.AllocatedQuantity(line.QtyNeeded, line.QtyOnHand);
            if (quantity <= 0)
                continue;

            int position = 0;
            while (position < result.Count)
            {
                int aisleOrder = string.Compare(result[position].Aisle, line.Aisle);
                if (aisleOrder > 0 || (aisleOrder == 0 && result[position].Bin > line.Bin))
                    break;
                position++;
            }
            result.Insert(position, (line.Aisle, line.Bin, line.Sku, quantity));
        }
        return result;
    }
}
