using api_design_assignment.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace api_design_assignment.Services;

public class WinesService
{
    private readonly IMongoCollection<Wine> _wines;

    public WinesService(IOptions<WineCellarDatabaseSettings> settings)
    {
        var mongoClient = new MongoClient(
            settings.Value.ConnectionString);

     //   var mongoDatabase = mongoClient.GetDatabase(
    //   settings.Value.DatabaseName);

   //     _winesCollection = mongoDatabase.GetCollection<Wine>(
    //        settings.Value.WineCollectionName);

        _wines = mongoClient.GetDatabase(settings.Value.DatabaseName)
            .GetCollection<Wine>(settings.Value.WineCollectionName);
    }

    public async Task<List<Wine>> GetAsync() =>
        await _wines.Find(_ => true).ToListAsync();

    public async Task<Wine?> GetAsync(string id) =>
        await _wines.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Wine newWine) =>
        await _wines.InsertOneAsync(newWine);

    public async Task UpdateAsync(string id, Wine updatedWine) =>
        await _wines.ReplaceOneAsync(x => x.Id == id, updatedWine);

    public async Task RemoveAsync(string id) =>
        await _wines.DeleteOneAsync(x => x.Id == id);
}