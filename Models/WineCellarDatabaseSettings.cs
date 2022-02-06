namespace api_design_assignment.Models
{
  public class WineCellarDatabaseSettings
  {
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string WineCollectionName { get; set; } = null!;
  }
}