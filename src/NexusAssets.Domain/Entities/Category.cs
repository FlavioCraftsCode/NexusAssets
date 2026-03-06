using System.Collections.Generic;

namespace NexusAssets.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    public virtual ICollection<Asset> Assets { get; set; } = new List<Asset>();
}