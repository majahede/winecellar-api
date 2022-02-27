using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api_design_assignment.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string Email { get; set; } = null!;

    public decimal Password { get; set; }

}