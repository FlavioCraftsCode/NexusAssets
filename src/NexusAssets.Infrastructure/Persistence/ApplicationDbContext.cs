using Microsoft.EntityFrameworkCore;
using NexusAssets.Domain.Entities; // Namespace correto para as entidades
using NexusAssets.Application.Common.Interfaces; 

namespace NexusAssets.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // Implementação dos DbSets (Exigidos pela Interface e pelo EF)
    public DbSet<Asset> Assets => Set<Asset>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Location> Locations => Set<Location>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Lógica de Auditoria Automática
        foreach (var entry in ChangeTracker.Entries<Asset>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.Created = DateTime.Now;
                entry.Entity.CreatedBy = "System_User"; 
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.LastModified = DateTime.Now;
                entry.Entity.LastModifiedBy = "System_User";
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração da Entidade Asset
        modelBuilder.Entity<Asset>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.SerialNumber).HasMaxLength(50);
            entity.Property(e => e.Value).HasPrecision(18, 2);
            entity.Property(e => e.Status).HasDefaultValue("Disponível").HasMaxLength(20);

            // Configuração dos novos relacionamentos
            entity.HasOne(a => a.Category)
                  .WithMany(c => c.Assets)
                  .HasForeignKey(a => a.CategoryId)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(a => a.Location)
                  .WithMany(l => l.Assets)
                  .HasForeignKey(a => a.LocationId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // Configurações para Category e Location
        modelBuilder.Entity<Category>(e => { e.HasKey(x => x.Id); e.Property(x => x.Name).IsRequired().HasMaxLength(50); });
        modelBuilder.Entity<Location>(e => { e.HasKey(x => x.Id); e.Property(x => x.Name).IsRequired().HasMaxLength(100); });
    }
}