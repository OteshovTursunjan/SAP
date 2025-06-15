
using SAP.Application.Model.BussinesPartner;

namespace SAP.Application.Service;

public  interface IBussinesPartnerService
{
    Task<BussinesPartnerResponseModel> CreatBussinesPartner(CreateBussinesPartnerModel createBussinesPartnerModel);
    Task<List<BussinesPartnerResponseModel>> GetAllBussinesPartner();
    Task<BussinesPartnerResponseModel> GetBussinesPartner(string id);
    Task<bool> DeleteBussinesPartner(string id);
    Task<List<BussinesPartnerResponseModel>> FilterBussinesPartnersByName(string name);
    Task<List<BussinesPartnerResponseModel>> FilterBussinesPartnersByAddress(string address);
    Task<List<BussinesPartnerResponseModel>> FilterBussinesPartnersByPhone(string phone);
    Task<bool> UpdateBussinesPartner(string cardCode, UpdateBussinesPartnerModel updateModel);
}
