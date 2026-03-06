using NexusAssets.Domain.Entities;

namespace NexusAssets.Application.Common.Interfaces;

public interface IAssetService
{
    
    Task<List<Asset>> GetActiveAssetsAsync();
    Task<int> CreateAssetAsync(Asset asset);
    Task UpdateAssetAsync(Asset asset);
    Task DeleteAssetAsync(int id);
    Task<byte[]> ExportToExcelAsync();

    
    Task<List<Category>> GetCategoriesAsync();
    Task CreateCategoryAsync(Category category);

    
    Task<List<Location>> GetLocationsAsync();
    Task CreateLocationAsync(Location location); 
}