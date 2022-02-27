using api_design_assignment.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace api_design_assignment.Services;

public class WinesService
{
    private readonly IMongoCollection<Wine> _winesCollection;

    public WinesService(
        IOptions<WineCellarDatabaseSettings> wineCellarDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            wineCellarDatabaseSettings.Value.ConnectionString);

       var mongoDatabase = mongoClient.GetDatabase(
      wineCellarDatabaseSettings.Value.DatabaseName);

       _winesCollection = mongoDatabase.GetCollection<Wine>(
           wineCellarDatabaseSettings.Value.WineCollectionName);
    }

    public async Task<List<Wine>> GetAsync() =>
        await _winesCollection.Find(_ => true).ToListAsync();

    public async Task<Wine?> GetAsync(string id) =>
        await _winesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Wine newWine) =>
        await _winesCollection.InsertOneAsync(newWine);

    public async Task UpdateAsync(string id, Wine updatedWine) =>
        await _winesCollection.ReplaceOneAsync(x => x.Id == id, updatedWine);

    public async Task RemoveAsync(string id) =>
        await _winesCollection.DeleteOneAsync(x => x.Id == id);
}