namespace api_design_assignment.Models
{
    public class WineCellarDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string WineCollectionName { get; set; } = null!;

        public string UserCollectionName { get; set; } = null!;
        
        public string WebhookCollectionName { get; set; } = null!;
        
        public string JwtKey { get; set; } = null!;
    }
}