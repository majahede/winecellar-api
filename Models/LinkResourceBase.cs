namespace api_design_assignment.Models;

public class LinkResourceBase
{
    public LinkResourceBase()
    {
    }
    public List<Link> Links { get; set; } = new List<Link>();
}