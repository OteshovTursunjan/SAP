
namespace SAP.Application.Model.Items;

public  class CreateItemsModel
{
    public required string ItemCode { get; set; }
    public required string ItemName { get; set; }
    public required string ItemType { get; set; }
    public required string U_TypeGroup { get; set; }
}
