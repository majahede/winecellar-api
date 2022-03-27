using System.ComponentModel.DataAnnotations;
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
        
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Type { get; set; } = null!;

        [Required]
        public string Grape { get; set; } = null!;

        [Required]
        public string Country { get; set; } = null!;

        [Required]
        public string Region { get; set; } = null!;

        [Required]
        public string Producer { get; set; } = null!;
    }
}