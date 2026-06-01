using InventoryManager.Integration.PowerAutomate.Models;
using InventoryManager.Integration.PowerAutomate.Services;
using InventoryManager.Integration.Salesforce.Models;
using InventoryManager.Integration.Salesforce.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;

namespace InventoryManager.Integration.PowerAutomate.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class PowerAutomateController(IDropBoxService dropBoxService) : ControllerBase {

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SupportTicketModel model) {
            var content = JsonConvert.SerializeObject(model, Formatting.Indented);
            await dropBoxService.UploadFileAsync(content, $"SupportTicket_{Guid.NewGuid()}");
            return Ok();
        }
    }
}
