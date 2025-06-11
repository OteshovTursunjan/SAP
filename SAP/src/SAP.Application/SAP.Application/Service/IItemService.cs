

using SAP.Application.Model.Items;

namespace SAP.Application.Service;

public interface IItemService
{
    Task<ItemsResponseModel> CreateItem(CreateItemsModel model);
    Task<ItemsResponseModel> GetItemById( string  id);
    Task<List<ItemsResponseModel>> GetAllItems();
    Task<bool> DeleteItem(string id);
  //  Task<bool> UpdateItem(string id, string newItemName);
}
