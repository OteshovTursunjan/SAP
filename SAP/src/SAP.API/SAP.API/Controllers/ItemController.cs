using Microsoft.AspNetCore.Mvc;
using SAP.Application.Model.Employees;
using SAP.Application.Model.Items;
using SAP.Application.Service;

namespace SAP.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ItemController : Controller
{
    private readonly IItemService _itemService;
    public ItemController(IItemService itemService)
    {
        _itemService = itemService;
    }

    [HttpPost("CreateItem")]
    [ProducesResponseType(typeof(ItemsResponseModel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateEmployee([FromBody] CreateItemsModel createItemsModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var res = await _itemService.CreateItem(createItemsModel);
            return StatusCode(StatusCodes.Status201Created, res);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error occurred while creating employee");
        }
    }
    [HttpGet("GetItems({id})")]
    [ProducesResponseType(typeof(ItemsResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetItemsById(string id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var res = await _itemService.GetItemById(id);
        return Ok(res);
    }
    [HttpGet("GetAllItems")]
    public async Task<IActionResult> GetAllItems()
    {

        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var res = await _itemService.GetAllItems();
        return Ok(res);
    }
    //[HttpPatch("UpdateItem({id})")]
    //public async Task<IActionResult> UpdateItem(string id,[FromBody]string name)
    //{
    //    if (!ModelState.IsValid)
    //        return BadRequest(ModelState);
    //    var res = await _itemService.UpdateItem(id, name);
    //    return Ok(res);
    //}


    [HttpDelete("DeleteItems{id}")]
    [ProducesResponseType(typeof(ItemsResponseModel), StatusCodes.Status204NoContent)]
   public async Task<IActionResult> DeleteItems(string id)
    {

        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var res = await _itemService.DeleteItem(id);
        return Ok(res);
    }
}