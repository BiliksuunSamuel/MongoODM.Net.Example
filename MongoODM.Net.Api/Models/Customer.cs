using System.ComponentModel.DataAnnotations;

namespace MongoODM.Net.Api.Models;

public class Customer:CustomerRequest
{
    public string? Id { get; set; }=Guid.NewGuid().ToString("N");
    public DateTime CreatedAt { get; set; }=DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}

public class CustomerRequest
{
    [Required(AllowEmptyStrings = false)]
    public string? Name { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string? Email { get; set; }

    public Address? Address { get; set; }
}

public class Address
{
    public string? City { get; set; }
    public string? Country { get; set; }
}