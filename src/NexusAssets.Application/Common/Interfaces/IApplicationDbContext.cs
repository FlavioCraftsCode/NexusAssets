using Microsoft.EntityFrameworkCore;
using NexusAssets.Domain.Entities; 

namespace NexusAssets.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Asset> Assets { get; }
    
    
    DbSet<Category> Categories { get; }
    DbSet<Location> Locations { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}