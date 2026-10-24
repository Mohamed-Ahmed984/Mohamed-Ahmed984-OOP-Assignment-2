namespace SrpLab;

public class WmsBatchXml
{
    private WarehousePickList _pickList;

    public WmsBatchXml(WarehousePickList pickList)
    {
        _pickList = pickList;
    }

    public string WmsXmlBatch(string batchId)
    {
        var parts = new List<string>();
        foreach (var allocation in new StockAllocation(_pickList).Allocate())
            parts.Add($"<line sku=\"{allocation.Sku}\" qty=\"{allocation.Allocated}\" />");
        return $"<batch id=\"{batchId}\">{string.Join("", parts)}</batch>";
    }
}
