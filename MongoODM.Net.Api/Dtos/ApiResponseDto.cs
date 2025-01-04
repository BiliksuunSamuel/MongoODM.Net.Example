namespace MongoODM.Net.Api.Dtos;

public class ApiResponseDto<T>
{
    public string? Code { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
}