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
            .GetCollection<Webhook>(settings.Value.WebhookCollectionName);
    }
    
    public async Task RegisterWebhook(Webhook webhook) {
        await _webhooks.InsertOneAsync(webhook);
    }
    
    public async Task<Webhook?> GetAsync(string id) =>
        await _webhooks.Find(x => x.Id == id).FirstOrDefaultAsync();
    
    public async Task<List<Webhook>> GetAllWebhooks() =>
        await _webhooks.Find(_ => true).ToListAsync();
    
    public async Task RemoveAsync(string? id) =>
        await _webhooks.DeleteOneAsync(x => x.Id == id);
}