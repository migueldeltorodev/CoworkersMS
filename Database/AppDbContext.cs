using System.Text.Json;
using ManagementSystem.Api.Common.Domain;
using ManagementSystem.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ManagementSystem.Api.Database;

public class AppDbContext : DbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public AppDbContext(
        DbContextOptions<AppDbContext> options, 
        IHttpContextAccessor httpContextAccessor) 
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<BookingHistory> BookingHistories => Set<BookingHistory>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.CreatedBy = GetCurrentUserId();
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedBy = GetCurrentUserId();
                    break;
            }
        
        var auditLogs = await BeforeSaveChanges();
        var result = await base.SaveChangesAsync(cancellationToken);
        await SaveAuditLogs(auditLogs);
        return result;
    }

    private async Task<List<AuditLog>> BeforeSaveChanges()
    {
        ChangeTracker.DetectChanges();
        var auditLogs = new List<AuditLog>();
        var userId = GetCurrentUserId();

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is AuditLog || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                continue;

            var auditLog = new AuditLog(
                entityType: entry.Entity.GetType().Name,
                entityId: (Guid)entry.Properties.Single(p => p.Metadata.Name == "Id").CurrentValue!,
                action: entry.State.ToString(),
                changes: JsonSerializer.Serialize(GetChanges(entry)),
                userId: userId);

            auditLogs.Add(auditLog);
        }

        return auditLogs;
    }

    private Dictionary<string, object?> GetChanges(EntityEntry entry)
    {
        var changes = new Dictionary<string, object?>();

        foreach (var property in entry.Properties)
        {
            if (property.IsTemporary || property.Metadata.Name == "RowVersion")
                continue;

            switch (entry.State)
            {
                case EntityState.Added:
                    changes[property.Metadata.Name] = property.CurrentValue;
                    break;
                case EntityState.Modified:
                    if (property.IsModified)
                    {
                        changes[$"{property.Metadata.Name}_Old"] = property.OriginalValue;
                        changes[$"{property.Metadata.Name}_New"] = property.CurrentValue;
                    }
                    break;
                case EntityState.Deleted:
                    changes[property.Metadata.Name] = property.OriginalValue;
                    break;
            }
        }

        return changes;
    }

    private string GetCurrentUserId()
    {
        // For getting the user Id from context
        return _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
    }

    private async Task SaveAuditLogs(List<AuditLog> auditLogs)
    {
        if (!auditLogs.Any()) return;

        // We use other context for avoid infinity cycles
        using var auditContext = new AppDbContext(
            new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(Database.GetConnectionString())
                .Options,
            _httpContextAccessor);

        auditContext.AuditLogs.AddRange(auditLogs);
        await auditContext.SaveChangesAsync();
    }
}