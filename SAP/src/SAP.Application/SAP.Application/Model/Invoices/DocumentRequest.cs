

using SAP.Application.Model.Items;
using SAP.Application.Service.lmpl;
using System.Text.Json.Serialization;
using static SAP.Application.Model.Invoices.DocumentRequest;

namespace SAP.Application.Model.Invoices;

public class DocumentRequest
{
    public string ItemCode { get; set; }
    [JsonConverter(typeof(IntOrStringToStringConverter))]
    public string Quantity { get; set; }
    public string TaxCode { get; set; }
    [JsonConverter(typeof(FlexibleStringConverter))]
    public string UnitPrice
    {
        get; set;
    }


    public class InvoicesResponseModel
    {
        
        public string CardCode { get; set; }
        public List<DocumentRequest> DocumentLines { get; set; }
    }

}
public class  InvoicesListWrapper
{
    public List<InvoicesResponseModel> Value { get; set; } = [];
}