using Microsoft.EntityFrameworkCore;

namespace QuimiosHub.Data;

public static class DatabaseExtensions
{
    public static void EnsureIndexes(this QuimiosDbContext context)
    {
        // This method documents the indexes created via OnModelCreating
        // Indexes are defined in QuimiosDbContext.OnModelCreating() using Fluent API

        // Sample indexes:
        // - folio_grd, cliente_grd, fecha_recep (composite unique)
        // - fecha_recep (for date range queries)
        // - fec_libera (for pending sample queries)
        // - cliente_grd (for client filtering)

        // Inventory indexes:
        // - code (unique)
        // - category (for category filtering)
        // - is_active (for active item queries)

        // User indexes:
        // - username (unique)
        // - is_active (for active user queries)
    }

    public static async Task<bool> CanConnectAsync(this QuimiosDbContext context)
    {
        try
        {
            return await context.Database.CanConnectAsync();
        }
        catch
        {
            return false;
        }
    }

    public static async Task<DatabaseHealthStatus> GetHealthStatusAsync(this QuimiosDbContext context)
    {
        var status = new DatabaseHealthStatus();

        try
        {
            status.CanConnect = await context.Database.CanConnectAsync();

            if (status.CanConnect)
            {
                status.SampleCount = await context.Samples.CountAsync();
                status.UserCount = await context.Users.CountAsync();
                status.InventoryItemCount = await context.InventoryItems.CountAsync();
                status.PendingSampleCount = await context.Samples.CountAsync(s => s.FecLibera == null);
                status.IsHealthy = true;
            }
        }
        catch (Exception ex)
        {
            status.IsHealthy = false;
            status.ErrorMessage = ex.Message;
        }

        return status;
    }
}

public class DatabaseHealthStatus
{
    public bool IsHealthy { get; set; }
    public bool CanConnect { get; set; }
    public int SampleCount { get; set; }
    public int UserCount { get; set; }
    public int InventoryItemCount { get; set; }
    public int PendingSampleCount { get; set; }
    public string? ErrorMessage { get; set; }
}
