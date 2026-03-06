using System;

namespace NexusAssets.Domain.Entities;

public class Asset
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    
    public string? AssetTag { get; set; }
    
    public string? SerialNumber { get; set; }
    
    public decimal Value { get; set; }
    
    public DateTime AcquisitionDate { get; set; }
    
    public string Status { get; set; } = "Disponível";

    
    
    
    public DateTime? WarrantyExpiration { get; set; }

    
    
    public int? CategoryId { get; set; }
    public virtual Category? Category { get; set; }

    public int? LocationId { get; set; }
    public virtual Location? Location { get; set; }

    

    public DateTime Created { get; set; }
    
    public string? CreatedBy { get; set; }
    
    public DateTime? LastModified { get; set; }
    
    public string? LastModifiedBy { get; set; }
}