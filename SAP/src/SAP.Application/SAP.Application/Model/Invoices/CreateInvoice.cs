

using SAP.Application.Service.lmpl;
using System.Text.Json.Serialization;

namespace SAP.Application.Model.Invoices;

public  class CreateInvoice
{
    public string ItemCode { get; set; }
   
    public decimal Quantity { get; set; }
    public string TaxCode { get; set; }
    
    public decimal UnitPrice
    {
        get; set;
    }


    public class CreateInvoicesModel
    {

        public string CardCode { get; set; }
        public List<CreateInvoice> DocumentLines { get; set; }
    }

}
