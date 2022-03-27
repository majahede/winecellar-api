using api_design_assignment.Models;
using api_design_assignment.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_design_assignment.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WebhookController : ControllerBase
{
    private readonly WebhookService _webhookService;
    
    public WebhookController(WebhookService webhookService) =>
        _webhookService = webhookService;
    
    [HttpPost]
    public async Task<IActionResult> RegisterWebhook(Webhook webhook)
    {

        await _webhookService.RegisterWebhook(webhook);

        return Created("Webhook was successfully registered", webhook);
    }
    
    [HttpGet]
    public async Task<List<Webhook>> GetAll() => await _webhookService.GetAllWebhooks();
    
    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var webhook = await _webhookService.GetAsync(id);

        if (webhook is null)
        {
            return NotFound($"No user with id {id} exists");
        }

        await _webhookService.RemoveAsync(webhook.Id);

        return NoContent();
    }
}