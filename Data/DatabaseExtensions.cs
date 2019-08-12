using Microsoft.EntityFrameworkCore;

namespace LIMSApi.Data;

public static class DatabaseExtensions
{
    public static void EnsureIndexes(this LIMSDbContext context)
    {
        // This method documents the indexes created via OnModelCreating
        // Indexes are defined in LIMSDbContext.OnModelCreating() using Fluent API

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

    public static async Task<bool> CanConnectAsync(this LIMSDbContext context)
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

    public static async Task<DatabaseHealthStatus> GetHealthStatusAsync(this LIMSDbContext context)
    {
        var status = new DatabaseHealthStatus();

        try
        {
            status.CanConnect = await context.Database.CanConnectAsync();

            if (status.CanConnect)
            {
                status.ExamCount = await context.Exams.CountAsync();
                status.UserCount = await context.Users.CountAsync();
                status.InventoryItemCount = await context.InventoryItems.CountAsync();
                status.PendingExamCount = await context.Exams.CountAsync(s => s.ValidatedAt == null);
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
    public int ExamCount { get; set; }
    public int UserCount { get; set; }
    public int InventoryItemCount { get; set; }
    public int PendingExamCount { get; set; }
    public string? ErrorMessage { get; set; }
}
