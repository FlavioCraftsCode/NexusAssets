using System.Collections.Generic;

namespace NexusAssets.Domain.Entities;

public class Location
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; 
    public string? Address { get; set; }
    
    public virtual ICollection<Asset> Assets { get; set; } = new List<Asset>();
}