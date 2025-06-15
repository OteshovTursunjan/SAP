

using SAP.Application.Model.Purchase;

namespace SAP.Application.Service;

public  interface IPurchaseService
{
    Task<PurchaseResponseModel> GetPurchaseById(int id);
    Task<List<PurchaseResponseModel>> GetAllPurchaseBy();
}
