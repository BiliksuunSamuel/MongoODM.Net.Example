namespace MongoODM.Net.Api.Dtos;

public class BaseFilter
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Query { get; set; }
}