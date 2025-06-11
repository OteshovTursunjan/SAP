

using SAP.Application.Model.Employees;

namespace SAP.Application.Model.Items;

public  class ItemsResponseModel
{
    public required string ItemCode { get; set; }
    public required string ItemName { get; set; }
    public required string ItemType { get; set; }
    public required string U_TypeGroup { get; set; }
    public string SalesItem {  get; set; }
}
public class ItemsListWrapper
{
    public List<ItemsResponseModel> Value { get; set; } = [];
}