using Microsoft.AspNetCore.Mvc;
using SAP.Application.Model.Invoices;
using SAP.Application.Service;
using static SAP.Application.Model.Invoices.CreateInvoice;
using static SAP.Application.Model.Invoices.DocumentRequest;

namespace SAP.API.Controllers;

public class InvoicesController : Controller
{
    private readonly IInvoicesService _invoicesService;
    public InvoicesController(IInvoicesService invoicesService)
    {
        _invoicesService = invoicesService;
    }
    [HttpPost("CreateInvoices")]
    public async Task<IActionResult> CreateInvoices([FromBody]CreateInvoicesModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var res = await _invoicesService.CreateInvoice(model);
        return Ok(res);
    }

    [HttpGet("GetInvoicesById({id})")]
    public async Task<IActionResult> GetInvoicesById(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var res = await _invoicesService.GetInvoiceById(id);
        return Ok(res);
    }

    [HttpGet("GetAllInvoices")]
    public async Task<IActionResult> GetAllInvoices()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var res = await _invoicesService.GetAllInvoices();
        return Ok(res);
    }
    [HttpPost("CloseInvoices({id})")]
    public async Task<IActionResult> CloseInvoices(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var res = await _invoicesService.CloseInvoices(id);
            return Ok(res);
    }
}
