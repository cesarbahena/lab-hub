using Microsoft.EntityFrameworkCore;
using QuimiOSHub.Models;

namespace QuimiOSHub.Data;

public class QuimiosDbContext : DbContext
{
    public QuimiosDbContext(DbContextOptions<QuimiosDbContext> options) : base(options)
    {
    }

    public DbSet<Sample> Samples { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Shift> Shifts { get; set; }
    public DbSet<ShiftHandover> ShiftHandovers { get; set; }
    public DbSet<PendingSample> PendingSamples { get; set; }
    public DbSet<InventoryItem> InventoryItems { get; set; }
    public DbSet<InventoryMovement> InventoryMovements { get; set; }
    public DbSet<Reagent> Reagents { get; set; }
    public DbSet<ConsumptionRecord> ConsumptionRecords { get; set; }
    public DbSet<CollectionRoute> CollectionRoutes { get; set; }
    public DbSet<RouteStop> RouteStops { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<CheckIn> CheckIns { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Sample unique constraint (matching ETL)
        modelBuilder.Entity<Sample>()
            .HasIndex(s => new { s.Folio, s.ClientId, s.ReceivedAt })
            .IsUnique()
            .HasDatabaseName("sample_unique_constraint");

        // Configure InventoryItem unique code
        modelBuilder.Entity<InventoryItem>()
            .HasIndex(i => i.Code)
            .IsUnique();

        // Configure Reagent unique code
        modelBuilder.Entity<Reagent>()
            .HasIndex(r => r.Code)
            .IsUnique();

        // Configure User unique username
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        // Add performance indexes
        modelBuilder.Entity<Sample>()
            .HasIndex(s => s.ReceivedAt);

        modelBuilder.Entity<Sample>()
            .HasIndex(s => s.ClientId);

        modelBuilder.Entity<InventoryItem>()
            .HasIndex(i => i.Category);

        modelBuilder.Entity<ShiftHandover>()
            .HasIndex(sh => sh.HandoverDate);

        modelBuilder.Entity<ConsumptionRecord>()
            .HasIndex(cr => cr.ConsumptionDate);

        // Configure decimal precision for inventory
        modelBuilder.Entity<InventoryItem>()
            .Property(i => i.CurrentStock)
            .HasPrecision(18, 2);

        modelBuilder.Entity<InventoryItem>()
            .Property(i => i.MinStock)
            .HasPrecision(18, 2);

        modelBuilder.Entity<InventoryItem>()
            .Property(i => i.MaxStock)
            .HasPrecision(18, 2);

        modelBuilder.Entity<InventoryMovement>()
            .Property(i => i.Quantity)
            .HasPrecision(18, 2);

        modelBuilder.Entity<ConsumptionRecord>()
            .Property(cr => cr.ResearchConsumption)
            .HasPrecision(18, 2);

        modelBuilder.Entity<ConsumptionRecord>()
            .Property(cr => cr.RepeatConsumption)
            .HasPrecision(18, 2);

        modelBuilder.Entity<ConsumptionRecord>()
            .Property(cr => cr.QCConsumption)
            .HasPrecision(18, 2);

        modelBuilder.Entity<ConsumptionRecord>()
            .Property(cr => cr.ManualConsumption)
            .HasPrecision(18, 2);

        modelBuilder.Entity<ConsumptionRecord>()
            .Property(cr => cr.CalibrationConsumption)
            .HasPrecision(18, 2);

        modelBuilder.Entity<ConsumptionRecord>()
            .Property(cr => cr.TotalConsumption)
            .HasPrecision(18, 2);

        // Configure relationships with cascade delete
        modelBuilder.Entity<ShiftHandover>()
            .HasMany(sh => sh.PendingSamples)
            .WithOne(ps => ps.ShiftHandover)
            .HasForeignKey(ps => ps.ShiftHandoverId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CollectionRoute>()
            .HasMany(cr => cr.RouteStops)
            .WithOne(rs => rs.CollectionRoute)
            .HasForeignKey(rs => rs.CollectionRouteId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<InventoryItem>()
            .HasMany(ii => ii.InventoryMovements)
            .WithOne(im => im.InventoryItem)
            .HasForeignKey(im => im.InventoryItemId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Reagent>()
            .HasMany(r => r.ConsumptionRecords)
            .WithOne(cr => cr.Reagent)
            .HasForeignKey(cr => cr.ReagentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Reagent>()
            .HasMany(r => r.InventoryItems)
            .WithOne(ii => ii.Reagent)
            .HasForeignKey(ii => ii.ReagentId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<ConsumptionRecord>()
            .HasMany(cr => cr.InventoryMovements)
            .WithOne(im => im.ConsumptionRecord)
            .HasForeignKey(im => im.ConsumptionRecordId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
