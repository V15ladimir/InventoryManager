using InventoryManager.Integration.Salesforce.Models;
using InventoryManager.Integration.Salesforce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManager.Integration.Salesforce.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SalesforceController(ISalesforceService salesforceService) : ControllerBase {

        [HttpPost("export")]
        public async Task<IActionResult> Export([FromBody] SalesforceExportModel model) {
            await salesforceService.ExportAsync(model);
            return Ok();
        }
    }
}
