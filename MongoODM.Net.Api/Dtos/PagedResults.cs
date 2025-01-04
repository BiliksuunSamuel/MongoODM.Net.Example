namespace MongoODM.Net.Api.Dtos;

public class PagedResults<T>
{
    public int Page { get; set; } 
    public int PageSize { get; set; }
    public int TotalPages { get; set; } 
    public int TotalCount { get; set; }
    public List<T> Results { get; set; }=new List<T>();
    
}

