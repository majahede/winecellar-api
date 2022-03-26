using api_design_assignment.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace api_design_assignment.Services;

public class WebhookService
{
    private readonly IMongoCollection<Webhook> _webhooks;
    
    public WebhookService(IOptions<WineCellarDatabaseSettings> settings)
    {
        var mongoClient = new MongoClient(
            settings.Value.ConnectionString);
        
        _webhooks = mongoClient.GetDatabase(settings.Value.DatabaseName)
            .GetCollection<Webhook>("webhooks");
    }
    
    public async Task RegisterWebhook(Webhook webhook) {
        await _webhooks.InsertOneAsync(webhook);
    }
    
    public async Task<List<Webhook>> GetAllWebhooks() =>
        await _webhooks.Find(_ => true).ToListAsync();
}