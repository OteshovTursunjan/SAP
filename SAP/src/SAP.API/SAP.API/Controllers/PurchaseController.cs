using Microsoft.AspNetCore.Mvc;
using SAP.Application.Service;

namespace SAP.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class PurchaseController : Controller
{
    private readonly IPurchaseService _purchaseService;
    public PurchaseController(IPurchaseService purchaseService)
    {
        _purchaseService = purchaseService;

    }
    [HttpGet("GetPurchase({id})")]
    public async Task<IActionResult> GetPurchaseById(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var res  = await _purchaseService.GetPurchaseById(id);
        return Ok(res);
    }
    [HttpGet("GetAllPurchase")]
    public async Task<IActionResult> GetAllPurchase()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var res = await _purchaseService.GetAllPurchaseBy();
        return Ok(res);
    }


}
