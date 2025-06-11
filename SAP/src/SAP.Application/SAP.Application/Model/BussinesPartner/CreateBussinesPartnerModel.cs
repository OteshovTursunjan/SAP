using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Application.Model.BussinesPartner;

public  class CreateBussinesPartnerModel
{
    public required string CardCode { get; set; }
    public required string CardName { get; set; }
    public required string CardType { get; set; }
    public string? Address { get; set; }
    public string? Phone1 { get; set; }
    public string? Phone2 { get; set; }
}
