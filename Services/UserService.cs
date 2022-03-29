using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api_design_assignment.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using BC = BCrypt.Net.BCrypt;

namespace api_design_assignment.Services;

public class UserService
{
    private readonly IMongoCollection<User> _users;
    private string _key;
    public UserService(IOptions<WineCellarDatabaseSettings> settings)
    {
        var mongoClient = new MongoClient(
            settings.Value.ConnectionString);

        _users = mongoClient.GetDatabase(settings.Value.DatabaseName)
            .GetCollection<User>(settings.Value.UserCollectionName);

        _key = settings.Value.JwtKey;
    }

    public async Task<List<User>> GetAsync() =>
        await _users.Find(_ => true).ToListAsync();

    public async Task<User?> GetAsync(string id) =>
        await _users.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(User newUser)
    {
        var isEmailUnique = _users.Find(x => x.Email == newUser.Email).FirstOrDefault();
        
        if (isEmailUnique != null)
        {
            throw new Exception("Email is already registered.");
        }
        
        await _users.InsertOneAsync(newUser);
    }
    
    public async Task UpdateAsync(string id, User updatedUser) =>
        await _users.ReplaceOneAsync(x => x.Id == id, updatedUser);

    public async Task RemoveAsync(string? id) =>
        await _users.DeleteOneAsync(x => x.Id == id);

    public string? Authenticate(string email, string password)
    {
        
        var user = _users.Find(x => x.Email == email).FirstOrDefault();

        if (user == null) throw new Exception("Wrong email or password");
        if (!BC.Verify(password, user.Password)) throw new Exception("Wrong email or password");
   
        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenKey = Encoding.ASCII.GetBytes(_key);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new (JwtRegisteredClaimNames.Email, user.Email),
                new (JwtRegisteredClaimNames.Sub, user.Id),
            }),

            Expires = DateTime.UtcNow.AddHours(1),

            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}