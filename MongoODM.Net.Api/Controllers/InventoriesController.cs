using Microsoft.AspNetCore.Mvc;
using MongoODM.Net.Api.Dtos;
using MongoODM.Net.Api.Models;
using MongoODM.Net.Api.Services;
#pragma warning disable CS8604 // Possible null reference argument.

namespace MongoODM.Net.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoriesController:ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public InventoriesController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateInventoryAsync([FromBody] InventoryRequest inventoryRequestDto)
    {
        var response = await _inventoryService.CreateInventoryAsync(inventoryRequestDto);
        return StatusCode(int.Parse(response.Code), response);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetInventoryByIdAsync(string id)
    {
        var response = await _inventoryService.GetInventoryByIdAsync(id);
        return StatusCode(int.Parse(response.Code), response);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateInventoryAsync(string id,[FromBody] InventoryRequest inventoryRequestDto)
    {
        var response = await _inventoryService.UpdateInventoryAsync(id, inventoryRequestDto);
        return StatusCode(int.Parse(response.Code), response);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetInventoriesAsync([FromQuery] BaseFilter filter)
    {
        var response = await _inventoryService.GetInventoriesAsync(filter);
        return StatusCode(int.Parse(response.Code), response);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInventoryAsync(string id)
    {
        var response = await _inventoryService.DeleteInventoryAsync(id);
        return StatusCode(int.Parse(response.Code), response);
    }
    
    [HttpPost("bulk")]
    public async Task<IActionResult> CreateInventoriesAsync([FromBody] List<InventoryRequest> inventoryRequestDtos)
    {
        var response = await _inventoryService.CreateInventoriesAsync(inventoryRequestDtos);
        return StatusCode(int.Parse(response.Code), response);
    }
}