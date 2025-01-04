using MongoODM.Net.Api.Dtos;
using MongoODM.Net.Api.Models;

namespace MongoODM.Net.Api.Services;

public interface ICustomerService
{
    Task<ApiResponseDto<Customer>> CreateCustomerAsync(CustomerRequest customerRequestDto);
    Task<ApiResponseDto<Customer>> GetCustomerByIdAsync(string id);
    Task<ApiResponseDto<Customer>> UpdateCustomerAsync(string id, CustomerRequest customerRequestDto);
    Task<ApiResponseDto<PagedResults<Customer>>> GetCustomersAsync(BaseFilter filter);
}