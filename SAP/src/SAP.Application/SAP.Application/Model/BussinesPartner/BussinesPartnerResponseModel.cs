

using SAP.Application.Model.Items;

namespace SAP.Application.Model.BussinesPartner;

public  class BussinesPartnerResponseModel
{
    public required string CardCode { get; set; }
    public  required string CardName { get; set; }
    public required string CardType { get; set; }
    public string? Address { get; set; }
    public string? Phone1 { get; set; }
    public string? Phone2 { get; set; }
}
public class BussinesPartnerListWrapper
{
    public List<BussinesPartnerResponseModel> Value { get; set; } = [];
}