using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api_design_assignment.Models;

public class User : LinkResourceBase
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [Required]
    [EmailAddress]
    [BsonElement("Email")]
    public string Email { get; set; } = null!;

    [Required]
    [BsonElement("Password")]
    public string Password { get; set; } = null!;

}