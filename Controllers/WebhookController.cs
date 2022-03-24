using api_design_assignment.Models;
using Microsoft.AspNetCore.Mvc;

namespace api_design_assignment.Controllers;

public class WebhookController : ControllerBase
{
    private readonly WebhookService _webhookService;
    
    [HttpPost]
    public async Task<IActionResult> RegisterWebhook()
    {
        
        await _webhookService.Register();

        return Ok("Webhook successfully registered");
    }
}