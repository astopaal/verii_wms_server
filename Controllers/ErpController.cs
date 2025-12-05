using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ErpController : ControllerBase
    {
        private readonly IErpService _erpService;

        public ErpController(IErpService erpService)
        {
            _erpService = erpService;
        }


        [HttpGet("getOnHandQuantities")]
        public async Task<ActionResult<ApiResponse<List<OnHandQuantityDto>>>> GetOnHandQuantities(
            [FromQuery] int? depoKodu = null,
            [FromQuery] string? stokKodu = null,
            [FromQuery] string? seriNo = null,
            [FromQuery] string? projeKodu = null)
        {
            var result = await _erpService.GetOnHandQuantitiesAsync(depoKodu, stokKodu, seriNo, projeKodu);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("getAllCustomers")]
        public async Task<ActionResult<ApiResponse<List<CariDto>>>> GetCaris([FromQuery] string? cariKodu = null)
        {
            var result = await _erpService.GetCarisAsync(cariKodu);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("getAllProducts")]
        public async Task<ActionResult<ApiResponse<List<StokDto>>>> GetStoks([FromQuery] string? stokKodu = null)
        {
            var result = await _erpService.GetStoksAsync(stokKodu);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("getAllWarehouses")]
        public async Task<ActionResult<ApiResponse<List<DepoDto>>>> GetDepos([FromQuery] short? depoKodu = null)
        {
            var result = await _erpService.GetDeposAsync(depoKodu);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("getAllProjects")]
        public async Task<ActionResult<ApiResponse<List<ProjeDto>>>> GetProjeler()
        {
            var result = await _erpService.GetProjelerAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("getStokBarcode")]
        public async Task<ActionResult<ApiResponse<List<StokBarcodeDto>>>> GetStokBarcode(
            [FromQuery] string bar,
            [FromQuery] int depoKodu,
            [FromQuery] int modul,
            [FromQuery] int kullaniciId,
            [FromQuery] string barkodGrubu,
            [FromQuery] int hareketTuru)
        {
            var result = await _erpService.GetStokBarcodeAsync(bar, depoKodu, modul, kullaniciId, barkodGrubu, hareketTuru);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("getBranches")]
        public async Task<ActionResult<ApiResponse<List<BranchDto>>>> GetBranches([FromQuery] int? branchNo = null)
        {
            var result = await _erpService.GetBranchesAsync(branchNo);
            return StatusCode(result.StatusCode, result);
        }


        [HttpGet("health-check")]
        [AllowAnonymous]
        public IActionResult HealthCheck()
        {
            var healthResponse = new { Status = "Healthy", Timestamp = DateTime.UtcNow };
            return StatusCode(200, healthResponse);
        }
        
    }
}
