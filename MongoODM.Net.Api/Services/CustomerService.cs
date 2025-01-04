using JwtAdapter.Extensions;
using Mapster;
using MongoDB.Driver.Linq;
using MongoODM.Net.Api.Dtos;
using MongoODM.Net.Api.Models;
using MongoODM.Net.Extensions;
// ReSharper disable All
#pragma warning disable CS8602 // Dereference of a possibly null reference.

namespace MongoODM.Net.Api.Services;

public class CustomerService:ICustomerService
{
    private readonly MongoRepository<Customer> _customerRepository;
    private readonly ILogger<CustomerService> _logger;
    public CustomerService(MongoRepository<Customer> customerRepository, ILogger<CustomerService> logger)
    {
        _customerRepository = customerRepository;
        _logger = logger;
    }
    public async Task<ApiResponseDto<Customer>> CreateCustomerAsync(CustomerRequest customerRequestDto)
    {
        try
        {
              var customer = await _customerRepository.GetAll().FirstOrDefaultAsync(x=>string.Equals(x.Email,customerRequestDto.Email,StringComparison.OrdinalIgnoreCase));
                if (customer != null)
                {
                    return new ApiResponseDto<Customer>
                    {
                        Code = StatusCodes.Status409Conflict.ToString(),
                        Message = "Customer with this email address already exists"
                    };
                }

                var doc = customerRequestDto.Adapt<Customer>();
               await _customerRepository.AddAsync(doc);
                return new ApiResponseDto<Customer>
                {
                    Code = "201",
                    Message = "Customer created successfully",
                    Data = doc
                };

        }
        catch (Exception e)
        {
            _logger.LogError(e,"Error creating customer while creating customer\n{Data}",
                customerRequestDto.ToJsonString());
            return new ApiResponseDto<Customer>
            {
                Code = "500",
                Message = "Sorry, an error occurred"
            };
        }
    }

    public async Task<ApiResponseDto<Customer>> GetCustomerByIdAsync(string id)
    {
        try
        {

            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer!=null)
            {
                return new ApiResponseDto<Customer>
                {
                    Code = "404",
                    Message = "Customer not found"
                };
            }

            return new ApiResponseDto<Customer>
            {
                Code = "200",
                Message = "Customer found",
                Data = customer
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e,"Error getting customer by id\n{Id}",id);
            return new ApiResponseDto<Customer>
            {
                Code = "500",
                Message = "Sorry, an error occurred"
            };
        }
    }

    public async Task<ApiResponseDto<Customer>> UpdateCustomerAsync(string id, CustomerRequest customerRequestDto)
    {
        try
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer is null)
            {
                return new ApiResponseDto<Customer>
                {
                    Code = "404",
                    Message = "Customer not found"
                };
            }

            var doc = customerRequestDto.Adapt(customer);
            doc.UpdatedAt = DateTime.UtcNow;
            var res = await _customerRepository.UpdateAsync(id, doc);
            if (res == 0)
            {
                return new ApiResponseDto<Customer>
                {
                    Code = "500",
                    Message = "Customer update failed"
                };
            }

            return new ApiResponseDto<Customer>
            {
                Code = "200",
                Message = "Customer updated successfully",
                Data = doc
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error updating customer by id\n{Id}", id);
            return new ApiResponseDto<Customer>
            {
                Code = "500",
                Message = "Sorry, an error occurred"
            };
        }
    }

    public async Task<ApiResponseDto<PagedResults<Customer>>> GetCustomersAsync(BaseFilter filter)
    {
        try
        {
            var query=_customerRepository.GetAll();
            if (!string.IsNullOrEmpty(filter.Query))
            {
                string queryText = filter.Query.ToLower();
                query = query.Where(x =>
                    x.Name.ToLower().Contains(queryText) ||
                    x.Email.ToLower().Contains(queryText));
            }

            var results = await query.ToPagedListAsync(Math.Abs(filter.Page - 1), filter.PageSize);
            return new ApiResponseDto<PagedResults<Customer>>
            {
                Code = "200",
                Message = "Customers fetched successfully",
                Data = new PagedResults<Customer>
                {
                    Results = results.Items,
                    Page = results.Page,
                    PageSize = results.PageSize,
                    TotalCount = results.TotalCount,
                    TotalPages = results.TotalPages
                }
            };

        }
        catch (Exception e)
        {
            _logger.LogError(e,"Error getting customers\n{Filter}",filter.ToJsonString());
            return new ApiResponseDto<PagedResults<Customer>>
            {
                Code = "500",
                Message = "Sorry, an error occurred"
            };
        }
    }
}