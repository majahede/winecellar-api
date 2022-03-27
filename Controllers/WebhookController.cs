using api_design_assignment.Models;
using api_design_assignment.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_design_assignment.Controllers;

[Authorize]
[ApiController]
[Route("api/webhook")]
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
}