using Microsoft.EntityFrameworkCore;
using NexusAssets.Application.Common.Interfaces;
using NexusAssets.Domain.Entities; 
using ClosedXML.Excel;

namespace NexusAssets.Infrastructure.Services;

public class AssetService : IAssetService
{
    private readonly IApplicationDbContext _context;

    public AssetService(IApplicationDbContext context)
    {
        _context = context;
    }

    

    public async Task<List<Asset>> GetActiveAssetsAsync()
    {
        return await _context.Assets
            .Include(a => a.Category) 
            .Include(a => a.Location) 
            .AsNoTracking()
            .OrderByDescending(a => a.AcquisitionDate)
            .ToListAsync();
    }

    public async Task<int> CreateAssetAsync(Asset asset)
    {
        asset.Created = DateTime.Now;
        _context.Assets.Add(asset);
        await _context.SaveChangesAsync(default);
        return asset.Id;
    }

    public async Task UpdateAssetAsync(Asset asset)
    {
        asset.LastModified = DateTime.Now;
        _context.Assets.Update(asset);
        await _context.SaveChangesAsync(default);
    }

    public async Task DeleteAssetAsync(int id)
    {
        var asset = await _context.Assets.FindAsync(id);
        if (asset != null)
        {
            _context.Assets.Remove(asset);
            await _context.SaveChangesAsync(default);
        }
    }

    

    public async Task<List<Category>> GetCategoriesAsync()
    {
        return await _context.Categories
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task CreateCategoryAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync(default);
    }

    

    public async Task<List<Location>> GetLocationsAsync()
    {
        return await _context.Locations
            .AsNoTracking()
            .OrderBy(l => l.Name)
            .ToListAsync();
    }

    
    public async Task CreateLocationAsync(Location location)
    {
        _context.Locations.Add(location);
        await _context.SaveChangesAsync(default);
    }

    

    public async Task<byte[]> ExportToExcelAsync()
    {
        var assets = await GetActiveAssetsAsync();
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Ativos");

        
        worksheet.Cell(1, 1).Value = "Tag";
        worksheet.Cell(1, 2).Value = "Nome";
        worksheet.Cell(1, 3).Value = "Categoria";
        worksheet.Cell(1, 4).Value = "Localização";
        worksheet.Cell(1, 5).Value = "Status";
        worksheet.Cell(1, 6).Value = "Valor";

        var header = worksheet.Range(1, 1, 1, 6);
        header.Style.Font.Bold = true;
        header.Style.Fill.BackgroundColor = XLColor.LightGray;

        for (int i = 0; i < assets.Count; i++)
        {
            var row = i + 2;
            var asset = assets[i];
            
            worksheet.Cell(row, 1).Value = asset.AssetTag;
            worksheet.Cell(row, 2).Value = asset.Name;
            worksheet.Cell(row, 3).Value = asset.Category?.Name ?? "Sem Categoria";
            worksheet.Cell(row, 4).Value = asset.Location?.Name ?? "Sem Localização";
            worksheet.Cell(row, 5).Value = asset.Status;
            worksheet.Cell(row, 6).Value = asset.Value;
            worksheet.Cell(row, 6).Style.NumberFormat.Format = "R$ #,##0.00";
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}