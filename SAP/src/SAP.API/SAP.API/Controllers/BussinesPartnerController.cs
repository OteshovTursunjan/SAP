using Microsoft.AspNetCore.Mvc;
using SAP.Application.Model.BussinesPartner;
using SAP.Application.Service;

namespace SAP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BussinesPartnerController : Controller
    {
       private readonly IBussinesPartnerService _bussinesPartnerService;
        public BussinesPartnerController(IBussinesPartnerService bussinesPartnerService)
        {
            _bussinesPartnerService = bussinesPartnerService;
        }

        [HttpPost("CreateBussinesPartner")]
        public async Task<IActionResult> CreateBussinesPartner(CreateBussinesPartnerModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var res = await _bussinesPartnerService.CreatBussinesPartner(model);
            return Ok(res);
        }

        [HttpGet("GetBussinesPartner({id})")]
        public async Task<IActionResult> GetBussinesPartnerId(string id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var res = await _bussinesPartnerService.GetBussinesPartner(id);
            return Ok(res);
        }


        [HttpGet("GetAllBussinesPartner")]
        public async Task<IActionResult> GetAllBussinesPartner()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var res = await _bussinesPartnerService.GetAllBussinesPartner();
            return Ok(res);
        }
        [HttpGet("FilterBussinesPartnerByPhone")]
        public async Task<IActionResult> GetBussinesPartnerByPhone(string phone)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var res = await _bussinesPartnerService.FilterBussinesPartnersByPhone(phone);
            return Ok(res);
        }
        [HttpGet("FilterBussinesPartnerByAddres")]
        public async Task<IActionResult> GetBussinesPartnerByAddres(string addres)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var res = await _bussinesPartnerService.FilterBussinesPartnersByAddress(addres);
            return Ok(res);
        }
        [HttpGet("FilterBussinesPartnerByName")]
        public async Task<IActionResult> GetBussinesPartnerByName(string name)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var res = await _bussinesPartnerService.FilterBussinesPartnersByName(name);
            return Ok(res);
        }
        [HttpPatch("UpdateBussinesPartner")]
        public async Task<IActionResult> UpdatePartner(string id,UpdateBussinesPartnerModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var res = await _bussinesPartnerService.UpdateBussinesPartner(id, model);
            return Ok(res);
        }
        [HttpDelete("DeleteBussinesPartner({id})")]
        public async Task<IActionResult> DeleteBussinesPartner(string id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var res = await _bussinesPartnerService.DeleteBussinesPartner(id);
            return Ok(res);
        }
    }
}
