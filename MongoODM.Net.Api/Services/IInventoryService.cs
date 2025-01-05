using MongoODM.Net.Api.Dtos;
using MongoODM.Net.Api.Models;

namespace MongoODM.Net.Api.Services;

public interface IInventoryService
{
    Task<ApiResponseDto<Inventory>> CreateInventoryAsync(InventoryRequest inventoryRequestDto);
    Task<ApiResponseDto<Inventory>> GetInventoryByIdAsync(string id);
    Task<ApiResponseDto<Inventory>> UpdateInventoryAsync(string id, InventoryRequest inventoryRequestDto);
    Task<ApiResponseDto<PagedResults<Inventory>>> GetInventoriesAsync(BaseFilter filter);
    Task<ApiResponseDto<Inventory>> DeleteInventoryAsync(string id);
    //create bulkk
    Task<ApiResponseDto<List<Inventory>>> CreateInventoriesAsync(List<InventoryRequest> inventoryRequestDtos);
}