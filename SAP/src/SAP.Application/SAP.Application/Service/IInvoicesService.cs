
using static SAP.Application.Model.Invoices.CreateInvoice;
using static SAP.Application.Model.Invoices.DocumentRequest;

namespace SAP.Application.Service;

public  interface IInvoicesService
{ 
    Task<InvoicesResponseModel> CreateInvoice(CreateInvoicesModel createInvoicesModel);
    Task<InvoicesResponseModel> GetInvoiceById(int id);
    Task<List<InvoicesResponseModel>> GetAllInvoices();
    Task<bool> CloseInvoices (int id);

}
