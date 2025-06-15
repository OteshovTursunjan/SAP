
using SAP.Application.Service.lmpl;
using System.Text.Json.Serialization;
using static SAP.Application.Model.Invoices.DocumentRequest;

namespace SAP.Application.Model.Purchase;

public  class PurchaseResponseModel
{
        public string CardCode { get; set; }
        public List<DocumentLines> DocumentLine { get; set; }
    public class DocumentLines
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
    }

}
public class PurchaseListWrapper
{
    public List<PurchaseResponseModel> Value { get; set; } = [];
}