using System.ComponentModel.DataAnnotations;

namespace MongoODM.Net.Api.Models;


public class Inventory:InventoryRequest
{
    public string? Id { get; set; }=Guid.NewGuid().ToString("N");
    public int SoldCount { get; set; }
    public DateTime CreatedAt { get; set; }=DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}

public class InventoryRequest
{
    [Required(AllowEmptyStrings = false)]
    public string? Name { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string? Description { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string? Category { get; set; }
}