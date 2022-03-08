using api_design_assignment.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace api_design_assignment.Services;

public class UserService
{
    private readonly IMongoCollection<User> _users;

    public UserService(IOptions<WineCellarDatabaseSettings> settings)
    {
        var mongoClient = new MongoClient(
            settings.Value.ConnectionString);

        _users = mongoClient.GetDatabase(settings.Value.DatabaseName)
            .GetCollection<User>(settings.Value.UserCollectionName);
    }

    public async Task<List<User>> GetAsync() =>
        await _users.Find(_ => true).ToListAsync();

    public async Task<User?> GetAsync(string id) =>
        await _users.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(User newUser) =>
        await _users.InsertOneAsync(newUser);

    public async Task UpdateAsync(string id, User updatedUser) =>
        await _users.ReplaceOneAsync(x => x.Id == id, updatedUser);

    public async Task RemoveAsync(string id) =>
        await _users.DeleteOneAsync(x => x.Id == id);
}