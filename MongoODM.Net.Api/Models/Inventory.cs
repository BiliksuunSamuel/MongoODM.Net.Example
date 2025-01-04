namespace MongoODM.Net.Api.Models;


public class Inventory
{
    public string? Id { get; set; }=Guid.NewGuid().ToString("N");
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public int SoldCount { get; set; }
    public DateTime CreatedAt { get; set; }=DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}