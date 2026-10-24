namespace SrpLab;

public class PickerInstructions
{
    private WarehousePickList _pickList;

    public PickerInstructions(WarehousePickList pickList)
    {
        _pickList = pickList;
    }

    public string PickerScript()
    {
        var steps = new List<string>();
        var route = new PickWalkingOrder(_pickList).WalkingOrder();
        for (int i = 0; i < route.Count; i++)
        {
            var step = route[i];
            steps.Add($"{i + 1}. Go aisle {step.Aisle} bin {step.Bin}: pick {step.Qty} × {step.Sku}");
        }
        var shortages = new List<string>();
        foreach (var allocation in new StockAllocation(_pickList).Allocate())
        {
            
            foreach (var line in _pickList.Lines)
            {
                if (line.Sku == allocation.Sku)
                {
                    if (allocation.Allocated < line.QtyNeeded)
                        shortages.Add(allocation.Sku);
                    break;
                }
            }
        }
        string warning = "SHORTAGES: none";
        if (shortages.Count > 0)
            warning = "SHORTAGES: " + string.Join(", ", shortages);
        return string.Join('\n', steps) + "\n" + warning;
    }
}
