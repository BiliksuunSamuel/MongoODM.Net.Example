using Microsoft.AspNetCore.Mvc;
using MongoODM.Net.Api.Dtos;
using MongoODM.Net.Api.Models;
using MongoODM.Net.Api.Services;
#pragma warning disable CS8604 // Possible null reference argument.

namespace MongoODM.Net.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }
    
    
    [HttpGet("{customerId}")]
    public async Task<IActionResult> GetCustomer(string customerId)
    {
        var response = await _customerService.GetCustomerByIdAsync(customerId);
        return StatusCode(int.Parse(response.Code), response);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CustomerRequest customerRequest)
    {
        var response = await _customerService.CreateCustomerAsync(customerRequest);
        return StatusCode(int.Parse(response.Code), response);
    }
    
    [HttpPut("{customerId}")]
    public async Task<IActionResult> UpdateCustomer(string customerId, [FromBody] CustomerRequest customerRequest)
    {
        var response = await _customerService.UpdateCustomerAsync(customerId, customerRequest);
        return StatusCode(int.Parse(response.Code), response);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetCustomers([FromQuery] BaseFilter filter)
    {
        var response = await _customerService.GetCustomersAsync(filter);
        return StatusCode(int.Parse(response.Code), response);
    }
}