using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api_design_assignment.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace api_design_assignment.Services;

public class UserService
{
    private readonly IMongoCollection<User> _users;
    private readonly string key;
    public UserService(IOptions<WineCellarDatabaseSettings> settings)
    {
        var mongoClient = new MongoClient(
            settings.Value.ConnectionString);

        _users = mongoClient.GetDatabase(settings.Value.DatabaseName)
            .GetCollection<User>(settings.Value.UserCollectionName);
        key = "keythatneedstobechanged";
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

    public string Authenticate(string email, string password)
    {
        var user = _users.Find(x => x.Email == email && x.Password == password).FirstOrDefault();
        //var user = GetAsync(id);
        if (user == null)
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenKey = Encoding.ASCII.GetBytes(key);
        Console.WriteLine("50" + tokenKey);
        
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            }),

            Expires = DateTime.UtcNow.AddHours(1),

            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        Console.WriteLine(token);
        return tokenHandler.WriteToken(token);
    }
}