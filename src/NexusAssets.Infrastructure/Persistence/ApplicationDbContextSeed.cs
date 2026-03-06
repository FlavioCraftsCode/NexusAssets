using Microsoft.EntityFrameworkCore;
using NexusAssets.Domain.Entities; // Endereço atualizado
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NexusAssets.Infrastructure.Persistence;

public static class ApplicationDbContextSeed
{
    public static async Task SeedSampleDataAsync(ApplicationDbContext context)
    {
        // 1. Seed de Categorias (Se não houver nenhuma)
        if (!context.Categories.Any())
        {
            context.Categories.AddRange(
                new Category { Name = "Eletrônicos", Description = "Computadores, Monitores e Periféricos" },
                new Category { Name = "Móveis", Description = "Cadeiras, Mesas e Armários" },
                new Category { Name = "Infraestrutura", Description = "Servidores, Racks e Switches" }
            );
            await context.SaveChangesAsync();
        }

        // 2. Seed de Localizações
        if (!context.Locations.Any())
        {
            context.Locations.AddRange(
                new Location { Name = "Sede Principal - Sala 101" },
                new Location { Name = "Almoxarifado Central" },
                new Location { Name = "Data Center - Rack A" }
            );
            await context.SaveChangesAsync();
        }

        // 3. Seed de Ativos (Associando aos IDs criados acima)
        if (!context.Assets.Any())
        {
            // Pegamos as categorias e locais para vincular
            var eletronicos = context.Categories.FirstOrDefault(c => c.Name == "Eletrônicos");
            var moveis = context.Categories.FirstOrDefault(c => c.Name == "Móveis");
            var infra = context.Categories.FirstOrDefault(c => c.Name == "Infraestrutura");
            var sede = context.Locations.FirstOrDefault(l => l.Name == "Sede Principal - Sala 101");

            context.Assets.AddRange(
                new Asset { 
                    Name = "MacBook Pro M2", 
                    SerialNumber = "APPLE-MX123", 
                    AssetTag = "NB-001",
                    Value = 12500.00m, 
                    AcquisitionDate = DateTime.Now.AddMonths(-6), 
                    Status = "Em Uso",
                    CategoryId = eletronicos?.Id,
                    LocationId = sede?.Id,
                    Created = DateTime.Now,
                    CreatedBy = "SystemSeed"
                },
                new Asset { 
                    Name = "Monitor Dell 27p 4K", 
                    SerialNumber = "DELL-99887", 
                    AssetTag = "MON-042",
                    Value = 3200.50m, 
                    AcquisitionDate = DateTime.Now.AddMonths(-2), 
                    Status = "Disponível",
                    CategoryId = eletronicos?.Id,
                    LocationId = sede?.Id,
                    Created = DateTime.Now,
                    CreatedBy = "SystemSeed"
                },
                new Asset { 
                    Name = "Servidor HP ProLiant", 
                    SerialNumber = "HP-SRV-01", 
                    AssetTag = "SRV-001",
                    Value = 45000.00m, 
                    AcquisitionDate = DateTime.Now.AddYears(-1), 
                    Status = "Manutenção",
                    CategoryId = infra?.Id,
                    Created = DateTime.Now,
                    CreatedBy = "SystemSeed"
                },
                new Asset { 
                    Name = "Cadeira Herman Miller", 
                    SerialNumber = "HM-AERON-05", 
                    AssetTag = "MOV-088",
                    Value = 9800.00m, 
                    AcquisitionDate = DateTime.Now.AddMonths(-12), 
                    Status = "Em Uso",
                    CategoryId = moveis?.Id,
                    LocationId = sede?.Id,
                    Created = DateTime.Now,
                    CreatedBy = "SystemSeed"
                }
            );

            await context.SaveChangesAsync();
        }
    }
}