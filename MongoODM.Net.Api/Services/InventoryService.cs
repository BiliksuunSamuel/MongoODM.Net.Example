using JwtAdapter.Extensions;
using Mapster;
using MongoODM.Net.Api.Dtos;
using MongoODM.Net.Api.Models;
using MongoODM.Net.Extensions;

// ReSharper disable All

namespace MongoODM.Net.Api.Services;

public class InventoryService:IInventoryService
{
    private readonly ILogger<InventoryService> _logger;
    private readonly MongoRepository<Inventory> _mongoRepository;

    public InventoryService(ILogger<InventoryService> logger, MongoRepository<Inventory> mongoRepository)
    {
        _logger = logger;
        _mongoRepository = mongoRepository;
    }

    public async Task<ApiResponseDto<Inventory>> CreateInventoryAsync(InventoryRequest inventoryRequestDto)
    {
        try
        {
            var inventory = inventoryRequestDto.Adapt<Inventory>();
            await _mongoRepository.AddAsync(inventory);
            return new ApiResponseDto<Inventory>
            {
                Code = "201",
                Message = "Inventory created successfully",
                Data = inventory
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e,"an error occurred while creating inventory\n{Data}",
                inventoryRequestDto.ToJsonString());
            return new ApiResponseDto<Inventory>
            {
                Code = "500",
                Message = "Sorry,an error occurred"
            };
        }
    }

    public async Task<ApiResponseDto<Inventory>> GetInventoryByIdAsync(string id)
    {
        try
        {
            var inventory = await _mongoRepository.GetByIdAsync(id);
            if (inventory is null)
            {
                return new ApiResponseDto<Inventory>
                {
                    Code = "404",
                    Message = "Inventory not found"
                };
            }

            return new ApiResponseDto<Inventory>
            {
                Code = "200",
                Message = "Inventory found",
                Data = inventory
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e,"an error occurred while getting inventory by id\n{Id}",id);
            return new ApiResponseDto<Inventory>
            {
                Code = "500",
                Message = "Sorry,an error occurred"
            };
        }
    }

    public async Task<ApiResponseDto<Inventory>> UpdateInventoryAsync(string id, InventoryRequest inventoryRequestDto)
    {
        try
        {
            var inventory = await _mongoRepository.GetByIdAsync(id);
            if (inventory is null)
            {
                return new ApiResponseDto<Inventory>
                {
                    Code = "404",
                    Message = "Inventory not found"
                };
            }

            inventory = inventoryRequestDto.Adapt(inventory);
            inventory.UpdatedAt=DateTime.UtcNow;
            await _mongoRepository.UpdateAsync(id, inventory);
            return new ApiResponseDto<Inventory>
            {
                Code = "200",
                Message = "Inventory updated successfully",
                Data = inventory
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e,"an error occurred while updating inventory\n{Data}",
                inventoryRequestDto.ToJsonString());
            return new ApiResponseDto<Inventory>
            {
                Code = "500",
                Message = "Sorry,an error occurred"
            };
        }
    }

    public async Task<ApiResponseDto<PagedResults<Inventory>>> GetInventoriesAsync(BaseFilter filter)
    {
        try
        {
            var query=_mongoRepository.GetAll();
            if (!string.IsNullOrWhiteSpace(filter.Query))
            {
                //cast to lower case before comparing, name,description,category, either of them must not be null
                query = query.Where(x =>
                    !string.IsNullOrWhiteSpace(x.Name) && x.Name.ToLower().Contains(filter.Query.ToLower()) ||
                    !string.IsNullOrWhiteSpace(x.Description) &&
                    x.Description.ToLower().Contains(filter.Query.ToLower()) ||
                    !string.IsNullOrWhiteSpace(x.Category) && x.Category.ToLower().Contains(filter.Query.ToLower()));

            }
            
            var data=await query.ToPagedListAsync(Math.Abs(filter.Page-1), filter.PageSize);
            var response = new PagedResults<Inventory>
            {
                Results = data.Items,
                TotalCount = data.TotalCount,
                TotalPages = data.TotalPages,
                Page = data.Page,
                PageSize = data.PageSize
            };

            return new ApiResponseDto<PagedResults<Inventory>>
            {
                Code = "200",
                Message = "Inventories found",
                Data = response
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e,"an error occurred while getting inventories");
            return new ApiResponseDto<PagedResults<Inventory>>
            {
                Code = "500",
                Message = "Sorry,an error occurred"
            };
        }
    }

    public async Task<ApiResponseDto<Inventory>> DeleteInventoryAsync(string id)
    {
        try
        {
            var inventory = await _mongoRepository.GetByIdAsync(id);
            if (inventory is null)
            {
                return new ApiResponseDto<Inventory>
                {
                    Code = "404",
                    Message = "Inventory not found"
                };
            }

            var results = await _mongoRepository.DeleteByIdAsync(id);
            if (results == 0)
            {
                return new ApiResponseDto<Inventory>
                {
                    Code = "424",
                    Message = "Inventory delete failed"
                };
            }

            return new ApiResponseDto<Inventory>
            {
                Code = "200",
                Message = "Inventory deleted successfully"
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "an error occurred while deleting inventory\n{Id}", id);
            return new ApiResponseDto<Inventory>
            {
                Code = "500",
                Message = "Sorry,an error occurred"
            };
        }
    }

    public async Task<ApiResponseDto<List<Inventory>>> CreateInventoriesAsync(List<InventoryRequest> inventoryRequestDtos)
    {
        try
        {
            var inventories = inventoryRequestDtos.Adapt<List<Inventory>>();
            await _mongoRepository.AddBulkAsync(inventories);
            return new ApiResponseDto<List<Inventory>>
            {
                Code = "201",
                Message = "Inventories created successfully",
                Data = inventories
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "an error occurred while creating inventories\n{Data}",
                inventoryRequestDtos.ToJsonString());
            return new ApiResponseDto<List<Inventory>>
            {
                Code = "500",
                Message = "Sorry,an error occurred"
            };
        }
    }
}