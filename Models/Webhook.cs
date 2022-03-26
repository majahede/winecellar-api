using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api_design_assignment.Models;

public class Webhook
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Url { get; set; } = null!;

    public string Secret { get; set; } = null!;
    
}