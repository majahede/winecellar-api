using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api_design_assignment.Models
{
    public class Wine : LinkResourceBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? UserId { get; set; }

        [BsonElement("Name")]
        [JsonPropertyName("Name")]
        public string WineName { get; set; } = null!;

        public decimal Price { get; set; }

        public string Type { get; set; } = null!;

        public string Grape { get; set; } = null!;

        public string Country { get; set; } = null!;

        public string Region { get; set; } = null!;

        public string Producer { get; set; } = null!;
    }
}