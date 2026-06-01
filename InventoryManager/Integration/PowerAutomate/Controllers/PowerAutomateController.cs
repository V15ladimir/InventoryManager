using InventoryManager.Integration.PowerAutomate.Models;
using InventoryManager.Integration.PowerAutomate.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InventoryManager.Integration.PowerAutomate.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class PowerAutomateController(IDropBoxService dropBoxService) : ControllerBase {

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] SupportTicketModel model) {
            var content = JsonConvert.SerializeObject(model, Formatting.Indented);
            await dropBoxService.UploadFileAsync(content, $"{Guid.NewGuid()}");
            return Ok();
        }
    }
}
