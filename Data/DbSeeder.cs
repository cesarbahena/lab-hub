using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuimiOSHub.Models;
using QuimiOSHub.Services;

namespace QuimiOSHub.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(QuimiosDbContext context, ILogger logger)
    {
        logger.LogInformation("Starting database seeding...");

        try
        {
            await SeedUsersAsync(context, logger);
            await SeedShiftsAsync(context, logger);
            await SeedReagentsAsync(context, logger);
            await SeedInventoryItemsAsync(context, logger);
            await SeedSamplesFromCsvAsync(context, logger);
            await SeedInventoryMovementsAsync(context, logger);
            await SeedConsumptionRecordsAsync(context, logger);

            logger.LogInformation("Database seeding completed successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during database seeding");
            throw;
        }
    }

    private static async Task SeedUsersAsync(QuimiosDbContext context, ILogger logger)
    {
        if (await context.Users.AnyAsync())
        {
            logger.LogInformation("Users already exist, skipping");
            return;
        }

        logger.LogInformation("Seeding users...");

        var defaultPasswordHash = AuthService.HashPassword("password");

        var users = new List<User>
        {
            new User
            {
                Username = "jperez",
                FullName = "Juan Perez",
                Email = "jperez@lab.com",
                Role = "Analyst",
                PasswordHash = defaultPasswordHash,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                Username = "mgomez",
                FullName = "Maria Gomez",
                Email = "mgomez@lab.com",
                Role = "Supervisor",
                PasswordHash = defaultPasswordHash,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                Username = "rlopez",
                FullName = "Roberto Lopez",
                Email = "rlopez@lab.com",
                Role = "Technician",
                PasswordHash = defaultPasswordHash,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                Username = "amartinez",
                FullName = "Ana Martinez",
                Email = "amartinez@lab.com",
                Role = "Manager",
                PasswordHash = defaultPasswordHash,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                Username = "cgarcia",
                FullName = "Carlos Garcia",
                Email = "cgarcia@lab.com",
                Role = "Analyst",
                PasswordHash = defaultPasswordHash,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

        context.Users.AddRange(users);
        await context.SaveChangesAsync();

        logger.LogInformation("Seeded {Count} users (password: 'password')", users.Count);
    }

    private static async Task SeedShiftsAsync(QuimiosDbContext context, ILogger logger)
    {
        if (await context.Shifts.AnyAsync())
        {
            logger.LogInformation("Shifts already exist, skipping");
            return;
        }

        logger.LogInformation("Seeding shifts...");

        var shifts = new List<Shift>
        {
            new Shift
            {
                Name = "Matutino",
                StartTime = new TimeSpan(7, 0, 0),
                EndTime = new TimeSpan(15, 0, 0),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Shift
            {
                Name = "Vespertino",
                StartTime = new TimeSpan(15, 0, 0),
                EndTime = new TimeSpan(23, 0, 0),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Shift
            {
                Name = "Nocturno",
                StartTime = new TimeSpan(23, 0, 0),
                EndTime = new TimeSpan(7, 0, 0),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

        context.Shifts.AddRange(shifts);
        await context.SaveChangesAsync();

        logger.LogInformation("Seeded {Count} shifts", shifts.Count);
    }

    private static async Task SeedReagentsAsync(QuimiosDbContext context, ILogger logger)
    {
        if (await context.Reagents.AnyAsync())
        {
            logger.LogInformation("Reagents already exist, skipping");
            return;
        }

        logger.LogInformation("Seeding reagents with calibration values...");

        var reagents = new List<Reagent>
        {
            new Reagent { Code = "ACVALPMT", Name = "AC. VALP", CalibrationConsumption = 12, Category = "Chemistry", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "AFP_MTY", Name = "AFP", CalibrationConsumption = 12, Category = "Hormones", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "BHCGMTY", Name = "BETA-HCG", CalibrationConsumption = 12, Category = "Hormones", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "CA125MTY", Name = "CA-125", CalibrationConsumption = 12, Category = "Tumor Markers", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "CA153MTY", Name = "CA-153", CalibrationConsumption = 12, Category = "Tumor Markers", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "CA199MTY", Name = "CA-199", CalibrationConsumption = 12, Category = "Tumor Markers", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "CEA2MTY", Name = "CEA2", CalibrationConsumption = 12, Category = "Tumor Markers", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "CORSMTY", Name = "CORTISOL-OR", CalibrationConsumption = 12, Category = "Hormones", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "E2MTY", Name = "E2", CalibrationConsumption = 12, Category = "Hormones", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "FERR_MTY", Name = "FERR", CalibrationConsumption = 12, Category = "Chemistry", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "FSHMTY", Name = "FSH", CalibrationConsumption = 6, Category = "Hormones", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "INSULMTY", Name = "INSULINA", CalibrationConsumption = 12, Category = "Hormones", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "LHMTY", Name = "LH", CalibrationConsumption = 12, Category = "Hormones", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "PROGMTY", Name = "PROG", CalibrationConsumption = 4, Category = "Hormones", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "PROLMTY", Name = "PROL", CalibrationConsumption = 4, Category = "Hormones", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "PSALIBMT", Name = "PSA-LIBRE", CalibrationConsumption = 4, Category = "Tumor Markers", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "PSATOTMT", Name = "PSA-TOTAL", CalibrationConsumption = 4, Category = "Tumor Markers", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "TETOTMTY", Name = "TEST-TOTAL", CalibrationConsumption = 12, Category = "Hormones", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "TSHMTY", Name = "TSH", CalibrationConsumption = 4, Category = "Hormones", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "TUMTY", Name = "TU", CalibrationConsumption = 12, Category = "Hormones", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "T3LIBMTY", Name = "T3-LIBRE", CalibrationConsumption = 12, Category = "Hormones", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "T3TOTMTY", Name = "T3-TOTAL", CalibrationConsumption = 12, Category = "Hormones", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "T4LIBMTY", Name = "T4-LIBRE", CalibrationConsumption = 12, Category = "Hormones", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "T4TOTMTY", Name = "T4-TOTAL", CalibrationConsumption = 12, Category = "Hormones", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "ACURIMTY", Name = "AC. URICO-S", CalibrationConsumption = 6, Category = "Chemistry", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "ALBMTY", Name = "ALBUMINA-SUERO", CalibrationConsumption = 4, Category = "Chemistry", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "AMIMTY", Name = "AMILASA-S", CalibrationConsumption = 3, Category = "Chemistry", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "BILIDMTY", Name = "BILIRR-DIR", CalibrationConsumption = 6, Category = "Chemistry", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "BILITMTY", Name = "BILIRR-TOT", CalibrationConsumption = 6, Category = "Chemistry", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "CA-SMTY", Name = "Ca-S", CalibrationConsumption = 6, Category = "Chemistry", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "CLOMTY", Name = "CLORO-S", CalibrationConsumption = 6, Category = "Chemistry", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "COLHMTY", Name = "COLEST-HDL", CalibrationConsumption = 3, Category = "Chemistry", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "COLTMTY", Name = "COLEST-TOT", CalibrationConsumption = 6, Category = "Chemistry", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "CREAMTY", Name = "CREATININA-S", CalibrationConsumption = 3, Category = "Chemistry", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "C3_MTY", Name = "C3", CalibrationConsumption = 15, Category = "Immunology", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "C4_MTY", Name = "C4", CalibrationConsumption = 15, Category = "Immunology", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "DHLMTY", Name = "DHL", CalibrationConsumption = 0, Category = "Enzymes", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "FESMTY", Name = "FE SERICO", CalibrationConsumption = 3, Category = "Chemistry", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "FOSFAMTY", Name = "FOSF-ALCAL", CalibrationConsumption = 0, Category = "Enzymes", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "FOSFMTY", Name = "FOSFOR-S", CalibrationConsumption = 6, Category = "Chemistry", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "GGTPMTY", Name = "GGTP", CalibrationConsumption = 0, Category = "Enzymes", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "GLUMTY", Name = "GLUCOSA-S", CalibrationConsumption = 6, Category = "Chemistry", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "IgA_MTY", Name = "IgA", CalibrationConsumption = 18, Category = "Immunology", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "IgG_MTY", Name = "IgG", CalibrationConsumption = 15, Category = "Immunology", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "IgM_MTY", Name = "IgM", CalibrationConsumption = 15, Category = "Immunology", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "IgE_MTY", Name = "IgE", CalibrationConsumption = 10, Category = "Immunology", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "LIPASAMT", Name = "LIPASA", CalibrationConsumption = 3, Category = "Enzymes", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "MGSMTY", Name = "Mg-SERICO", CalibrationConsumption = 6, Category = "Chemistry", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "NITROMTY", Name = "NITROG URE", CalibrationConsumption = 6, Category = "Chemistry", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "PCRCUMTY", Name = "PCR-ULTRA", CalibrationConsumption = 12, Category = "Immunology", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "PCRULMTY", Name = "PCR-ULTRA", CalibrationConsumption = 12, Category = "Immunology", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "POTMTY", Name = "POTASIO-S", CalibrationConsumption = 6, Category = "Chemistry", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "PRTTSMTY", Name = "PROT-TOT-S", CalibrationConsumption = 6, Category = "Chemistry", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "SODMTY", Name = "SODIO-S", CalibrationConsumption = 6, Category = "Chemistry", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "TGOMTY", Name = "TGO", CalibrationConsumption = 0, Category = "Enzymes", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "TGPMTY", Name = "TGP", CalibrationConsumption = 0, Category = "Enzymes", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "TRF_MTY", Name = "TRF", CalibrationConsumption = 0, Category = "Chemistry", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "TRIGLMTY", Name = "TRIGLICERIDOS", CalibrationConsumption = 6, Category = "Chemistry", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "UIBCMTY", Name = "CAP. FIJ. Fe", CalibrationConsumption = 2, Category = "Chemistry", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "HBGLMTY", Name = "Hb-GLIC", CalibrationConsumption = 12, Category = "Chemistry", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Reagent { Code = "DIMEMTY", Name = "DIMERO D", CalibrationConsumption = 10, Category = "Coagulation", Unit = "Pruebas", IsActive = true, CreatedAt = DateTime.UtcNow }
        };

        context.Reagents.AddRange(reagents);
        await context.SaveChangesAsync();

        logger.LogInformation("Seeded {Count} reagents with calibration values", reagents.Count);
    }

    private static async Task SeedInventoryItemsAsync(QuimiosDbContext context, ILogger logger)
    {
        if (await context.InventoryItems.AnyAsync())
        {
            logger.LogInformation("Inventory items already exist, skipping");
            return;
        }

        logger.LogInformation("Seeding inventory items and linking to reagents...");

        // Load all reagents first to link inventory items
        var reagents = await context.Reagents.ToDictionaryAsync(r => r.Code, r => r.Id);

        var items = new List<InventoryItem>
        {
            // Hormones
            new InventoryItem { Code = "AFP_MTY", Name = "AFP", Category = "Hormones", Unit = "test", CurrentStock = 100, MinStock = 20, MaxStock = 200 },
            new InventoryItem { Code = "BHCGMTY", Name = "BETA-HCG", Category = "Hormones", Unit = "test", CurrentStock = 100, MinStock = 20, MaxStock = 200 },
            new InventoryItem { Code = "CORSMTY", Name = "CORTISOL-OR", Category = "Hormones", Unit = "test", CurrentStock = 50, MinStock = 10, MaxStock = 100 },
            new InventoryItem { Code = "E2MTY", Name = "E2", Category = "Hormones", Unit = "test", CurrentStock = 50, MinStock = 10, MaxStock = 100 },
            new InventoryItem { Code = "FSHMTY", Name = "FSH", Category = "Hormones", Unit = "test", CurrentStock = 50, MinStock = 10, MaxStock = 100 },
            new InventoryItem { Code = "INSULMTY", Name = "INSULINA", Category = "Hormones", Unit = "test", CurrentStock = 50, MinStock = 10, MaxStock = 100 },
            new InventoryItem { Code = "LHMTY", Name = "LH", Category = "Hormones", Unit = "test", CurrentStock = 50, MinStock = 10, MaxStock = 100 },
            new InventoryItem { Code = "PROGMTY", Name = "PROG", Category = "Hormones", Unit = "test", CurrentStock = 50, MinStock = 10, MaxStock = 100 },
            new InventoryItem { Code = "PROLMTY", Name = "PROL", Category = "Hormones", Unit = "test", CurrentStock = 50, MinStock = 10, MaxStock = 100 },
            new InventoryItem { Code = "TETOTMTY", Name = "TEST-TOTAL", Category = "Hormones", Unit = "test", CurrentStock = 50, MinStock = 10, MaxStock = 100 },
            new InventoryItem { Code = "TSHMTY", Name = "TSH", Category = "Hormones", Unit = "test", CurrentStock = 150, MinStock = 30, MaxStock = 300 },
            new InventoryItem { Code = "TUMTY", Name = "TU", Category = "Hormones", Unit = "test", CurrentStock = 50, MinStock = 10, MaxStock = 100 },
            new InventoryItem { Code = "T3LIBMTY", Name = "T3-LIBRE", Category = "Hormones", Unit = "test", CurrentStock = 50, MinStock = 10, MaxStock = 100 },
            new InventoryItem { Code = "T3TOTMTY", Name = "T3-TOTAL", Category = "Hormones", Unit = "test", CurrentStock = 50, MinStock = 10, MaxStock = 100 },
            new InventoryItem { Code = "T4LIBMTY", Name = "T4-LIBRE", Category = "Hormones", Unit = "test", CurrentStock = 100, MinStock = 20, MaxStock = 200 },
            new InventoryItem { Code = "T4TOTMTY", Name = "T4-TOTAL", Category = "Hormones", Unit = "test", CurrentStock = 50, MinStock = 10, MaxStock = 100 },

            // Tumor Markers
            new InventoryItem { Code = "CA125MTY", Name = "CA-125", Category = "Tumor Markers", Unit = "test", CurrentStock = 50, MinStock = 10, MaxStock = 100 },
            new InventoryItem { Code = "CA153MTY", Name = "CA-153", Category = "Tumor Markers", Unit = "test", CurrentStock = 50, MinStock = 10, MaxStock = 100 },
            new InventoryItem { Code = "CA199MTY", Name = "CA-199", Category = "Tumor Markers", Unit = "test", CurrentStock = 50, MinStock = 10, MaxStock = 100 },
            new InventoryItem { Code = "CEA2MTY", Name = "CEA2", Category = "Tumor Markers", Unit = "test", CurrentStock = 50, MinStock = 10, MaxStock = 100 },
            new InventoryItem { Code = "PSALIBMT", Name = "PSA-LIBRE", Category = "Tumor Markers", Unit = "test", CurrentStock = 100, MinStock = 20, MaxStock = 200 },
            new InventoryItem { Code = "PSATOTMT", Name = "PSA-TOTAL", Category = "Tumor Markers", Unit = "test", CurrentStock = 150, MinStock = 30, MaxStock = 300 },

            // Chemistry
            new InventoryItem { Code = "ACVALPMT", Name = "AC. VALP", Category = "Chemistry", Unit = "test", CurrentStock = 50, MinStock = 10, MaxStock = 100 },
            new InventoryItem { Code = "ACURIMTY", Name = "AC. URICO-S", Category = "Chemistry", Unit = "test", CurrentStock = 150, MinStock = 30, MaxStock = 300 },
            new InventoryItem { Code = "ALBMTY", Name = "ALBUMINA-SUERO", Category = "Chemistry", Unit = "test", CurrentStock = 150, MinStock = 30, MaxStock = 300 },
            new InventoryItem { Code = "AMIMTY", Name = "AMILASA-S", Category = "Chemistry", Unit = "test", CurrentStock = 100, MinStock = 20, MaxStock = 200 },
            new InventoryItem { Code = "BILIDMTY", Name = "BILIRR-DIR", Category = "Chemistry", Unit = "test", CurrentStock = 100, MinStock = 20, MaxStock = 200 },
            new InventoryItem { Code = "BILITMTY", Name = "BILIRR-TOT", Category = "Chemistry", Unit = "test", CurrentStock = 100, MinStock = 20, MaxStock = 200 },
            new InventoryItem { Code = "CA-SMTY", Name = "Ca-S", Category = "Chemistry", Unit = "test", CurrentStock = 150, MinStock = 30, MaxStock = 300 },
            new InventoryItem { Code = "CLOMTY", Name = "CLORO-S", Category = "Chemistry", Unit = "test", CurrentStock = 200, MinStock = 40, MaxStock = 400 },
            new InventoryItem { Code = "COLHMTY", Name = "COLEST-HDL", Category = "Chemistry", Unit = "test", CurrentStock = 200, MinStock = 40, MaxStock = 400 },
            new InventoryItem { Code = "COLTMTY", Name = "COLEST-TOT", Category = "Chemistry", Unit = "test", CurrentStock = 200, MinStock = 40, MaxStock = 400 },
            new InventoryItem { Code = "CREAMTY", Name = "CREATININA-S", Category = "Chemistry", Unit = "test", CurrentStock = 200, MinStock = 40, MaxStock = 400 },
            new InventoryItem { Code = "C3_MTY", Name = "C3", Category = "Immunology", Unit = "test", CurrentStock = 50, MinStock = 10, MaxStock = 100 },
            new InventoryItem { Code = "C4_MTY", Name = "C4", Category = "Immunology", Unit = "test", CurrentStock = 50, MinStock = 10, MaxStock = 100 },
            new InventoryItem { Code = "DHLMTY", Name = "DHL", Category = "Enzymes", Unit = "test", CurrentStock = 150, MinStock = 30, MaxStock = 300 },
            new InventoryItem { Code = "FESMTY", Name = "FE SERICO", Category = "Chemistry", Unit = "test", CurrentStock = 100, MinStock = 20, MaxStock = 200 },
            new InventoryItem { Code = "FOSFAMTY", Name = "FOSF-ALCAL", Category = "Enzymes", Unit = "test", CurrentStock = 150, MinStock = 30, MaxStock = 300 },
            new InventoryItem { Code = "FOSFMTY", Name = "FOSFOR-S", Category = "Chemistry", Unit = "test", CurrentStock = 100, MinStock = 20, MaxStock = 200 },
            new InventoryItem { Code = "GGTPMTY", Name = "GGTP", Category = "Enzymes", Unit = "test", CurrentStock = 150, MinStock = 30, MaxStock = 300 },
            new InventoryItem { Code = "GLUMTY", Name = "GLUCOSA-S", Category = "Chemistry", Unit = "test", CurrentStock = 300, MinStock = 60, MaxStock = 600 },
            new InventoryItem { Code = "IgA_MTY", Name = "IgA", Category = "Immunology", Unit = "test", CurrentStock = 50, MinStock = 10, MaxStock = 100 },
            new InventoryItem { Code = "IgG_MTY", Name = "IgG", Category = "Immunology", Unit = "test", CurrentStock = 50, MinStock = 10, MaxStock = 100 },
            new InventoryItem { Code = "IgM_MTY", Name = "IgM", Category = "Immunology", Unit = "test", CurrentStock = 50, MinStock = 10, MaxStock = 100 },
            new InventoryItem { Code = "IgE_MTY", Name = "IgE", Category = "Immunology", Unit = "test", CurrentStock = 50, MinStock = 10, MaxStock = 100 },
            new InventoryItem { Code = "LIPASAMT", Name = "LIPASA", Category = "Enzymes", Unit = "test", CurrentStock = 100, MinStock = 20, MaxStock = 200 },
            new InventoryItem { Code = "MGSMTY", Name = "Mg-SERICO", Category = "Chemistry", Unit = "test", CurrentStock = 100, MinStock = 20, MaxStock = 200 },
            new InventoryItem { Code = "NITROMTY", Name = "NITROG URE", Category = "Chemistry", Unit = "test", CurrentStock = 150, MinStock = 30, MaxStock = 300 },
            new InventoryItem { Code = "PCRCUMTY", Name = "PCR-ULTRA", Category = "Immunology", Unit = "test", CurrentStock = 100, MinStock = 20, MaxStock = 200 },
            new InventoryItem { Code = "PCRULMTY", Name = "PCR-ULTRA", Category = "Immunology", Unit = "test", CurrentStock = 100, MinStock = 20, MaxStock = 200 },
            new InventoryItem { Code = "POTMTY", Name = "CLORO-S", Category = "Chemistry", Unit = "test", CurrentStock = 200, MinStock = 40, MaxStock = 400 },
            new InventoryItem { Code = "PRTTSMTY", Name = "PROT-TOT-S", Category = "Chemistry", Unit = "test", CurrentStock = 150, MinStock = 30, MaxStock = 300 },
            new InventoryItem { Code = "SODMTY", Name = "CLORO-S", Category = "Chemistry", Unit = "test", CurrentStock = 200, MinStock = 40, MaxStock = 400 },
            new InventoryItem { Code = "TGOMTY", Name = "TGO", Category = "Enzymes", Unit = "test", CurrentStock = 200, MinStock = 40, MaxStock = 400 },
            new InventoryItem { Code = "TGPMTY", Name = "TGP", Category = "Enzymes", Unit = "test", CurrentStock = 200, MinStock = 40, MaxStock = 400 },
            new InventoryItem { Code = "TRF_MTY", Name = "TRF", Category = "Chemistry", Unit = "test", CurrentStock = 50, MinStock = 10, MaxStock = 100 },
            new InventoryItem { Code = "TRIGLMTY", Name = "TRIGLICERIDOS", Category = "Chemistry", Unit = "test", CurrentStock = 200, MinStock = 40, MaxStock = 400 },
            new InventoryItem { Code = "UIBCMTY", Name = "CAP. FIJ. Fe", Category = "Chemistry", Unit = "test", CurrentStock = 50, MinStock = 10, MaxStock = 100 },
            new InventoryItem { Code = "FERR_MTY", Name = "FERR", Category = "Chemistry", Unit = "test", CurrentStock = 100, MinStock = 20, MaxStock = 200 },
            new InventoryItem { Code = "HBGLMTY", Name = "Hb-GLIC", Category = "Chemistry", Unit = "test", CurrentStock = 150, MinStock = 30, MaxStock = 300 },
            new InventoryItem { Code = "DIMEMTY", Name = "DIMERO D", Category = "Coagulation", Unit = "test", CurrentStock = 100, MinStock = 20, MaxStock = 200 }
        };

        // Link inventory items to reagents by code
        foreach (var item in items)
        {
            if (reagents.TryGetValue(item.Code, out var reagentId))
            {
                item.ReagentId = reagentId;
            }
        }

        context.InventoryItems.AddRange(items);
        await context.SaveChangesAsync();

        var linked = items.Count(i => i.ReagentId != null);
        logger.LogInformation("Seeded {Count} inventory items ({Linked} linked to reagents)", items.Count, linked);
    }

    private static async Task SeedSamplesFromCsvAsync(QuimiosDbContext context, ILogger logger)
    {
        if (await context.Samples.AnyAsync())
        {
            logger.LogInformation("Samples already exist, skipping");
            return;
        }

        logger.LogInformation("Seeding samples from Muestras.csv structure...");

        var samples = new List<Sample>();
        var baseDate = DateTime.UtcNow.AddDays(-3);

        // Completed samples (for reference)
        samples.AddRange(new[]
        {
            new Sample
            {
                CreatedAt = baseDate.AddDays(-10).AddHours(9).AddMinutes(15),
                ReceivedAt = baseDate.AddDays(-10).AddHours(10).AddMinutes(30),
                Folio = 20001,
                ClientId = 101,
                PatientId = 1001,
                ExamId = 303,
                ExamName = "Quimica Sanguinea",
                ProcessedAt = baseDate.AddDays(-10).AddHours(14).AddMinutes(20),
                ValidatedAt = baseDate.AddDays(-10).AddHours(16).AddMinutes(45),
                Location = "Monterrey",
                Outsourcer = "Quest Labs",
                Priority = "Normal",
                BirthDate = new DateTime(1985, 3, 15, 0, 0, 0, DateTimeKind.Utc)
            },
            new Sample
            {
                CreatedAt = baseDate.AddDays(-8).AddHours(10).AddMinutes(45),
                ReceivedAt = baseDate.AddDays(-8).AddHours(11).AddMinutes(20),
                Folio = 20002,
                ClientId = 102,
                PatientId = 1002,
                ExamId = 304,
                ExamName = "Perfil Tiroideo",
                ProcessedAt = baseDate.AddDays(-8).AddHours(13).AddMinutes(10),
                ValidatedAt = baseDate.AddDays(-8).AddHours(15).AddMinutes(30),
                Location = "Celaya",
                Outsourcer = "LabCorp",
                Priority = "Normal",
                BirthDate = new DateTime(1990, 7, 22, 0, 0, 0, DateTimeKind.Utc),
            }
        });

        // PENDING SAMPLES - 12 samples for ShiftCheck testing
        samples.AddRange(new[]
        {
            new Sample
            {
                CreatedAt = baseDate.AddHours(8).AddMinutes(30),
                ReceivedAt = baseDate.AddHours(9).AddMinutes(15),
                Folio = 20101,
                ClientId = 101,
                PatientId = 2001,
                ExamId = 305,
                ExamName = "Biometria Hematica",
                ProcessedAt = baseDate.AddHours(11).AddMinutes(45),
                ValidatedAt = null,
                Location = "Monterrey",
                Outsourcer = "Quest Labs",
                Priority = "Urgente",
                BirthDate = new DateTime(1978, 5, 10, 0, 0, 0, DateTimeKind.Utc),
            },
            new Sample
            {
                CreatedAt = baseDate.AddHours(9).AddMinutes(20),
                ReceivedAt = baseDate.AddHours(10).AddMinutes(5),
                Folio = 20102,
                ClientId = 102,
                PatientId = 2002,
                ExamId = 306,
                ExamName = "Glucosa",
                ProcessedAt = baseDate.AddHours(12).AddMinutes(30),
                ValidatedAt = null,
                Location = "Celaya",
                Outsourcer = "LabCorp",
                Priority = "Normal",
                BirthDate = new DateTime(1992, 11, 8, 0, 0, 0, DateTimeKind.Utc),
            },
            new Sample
            {
                CreatedAt = baseDate.AddDays(1).AddHours(7).AddMinutes(45),
                ReceivedAt = baseDate.AddDays(1).AddHours(8).AddMinutes(30),
                Folio = 20103,
                ClientId = 101,
                PatientId = 2003,
                ExamId = 307,
                ExamName = "Perfil Hepatico",
                ProcessedAt = null,
                ValidatedAt = null,
                Location = "Monterrey",
                Outsourcer = "Quest Labs",
                Priority = "Normal",
                BirthDate = new DateTime(1965, 2, 14, 0, 0, 0, DateTimeKind.Utc),
            },
            new Sample
            {
                CreatedAt = baseDate.AddDays(1).AddHours(10).AddMinutes(10),
                ReceivedAt = baseDate.AddDays(1).AddHours(11).AddMinutes(0),
                Folio = 20104,
                ClientId = 103,
                PatientId = 2004,
                ExamId = 308,
                ExamName = "Examen General de Orina",
                ProcessedAt = baseDate.AddDays(1).AddHours(13).AddMinutes(20),
                ValidatedAt = null,
                Location = "Tijuana",
                Outsourcer = "Maq X",
                Priority = "Stat",
                BirthDate = new DateTime(1988, 9, 3, 0, 0, 0, DateTimeKind.Utc),
            },
            new Sample
            {
                CreatedAt = baseDate.AddDays(1).AddHours(14).AddMinutes(25),
                ReceivedAt = baseDate.AddDays(1).AddHours(15).AddMinutes(10),
                Folio = 20105,
                ClientId = 102,
                PatientId = 2005,
                ExamId = 309,
                ExamName = "Creatinina",
                ProcessedAt = null,
                ValidatedAt = null,
                Location = "Celaya",
                Outsourcer = "LabCorp",
                Priority = "Normal",
                BirthDate = new DateTime(1995, 6, 18, 0, 0, 0, DateTimeKind.Utc),
            },
            new Sample
            {
                CreatedAt = baseDate.AddDays(2).AddHours(8).AddMinutes(0),
                ReceivedAt = baseDate.AddDays(2).AddHours(9).AddMinutes(0),
                Folio = 20106,
                ClientId = 104,
                PatientId = 2006,
                ExamId = 310,
                ExamName = "Colesterol Total",
                ProcessedAt = baseDate.AddDays(2).AddHours(11).AddMinutes(15),
                ValidatedAt = null,
                Location = "Puebla",
                Outsourcer = "Maq Y",
                Priority = "Normal",
                BirthDate = new DateTime(1982, 1, 27, 0, 0, 0, DateTimeKind.Utc),
            },
            new Sample
            {
                CreatedAt = baseDate.AddDays(2).AddHours(9).AddMinutes(40),
                ReceivedAt = baseDate.AddDays(2).AddHours(10).AddMinutes(30),
                Folio = 20107,
                ClientId = 101,
                PatientId = 2007,
                ExamId = 311,
                ExamName = "Trigliceridos",
                ProcessedAt = baseDate.AddDays(2).AddHours(12).AddMinutes(45),
                ValidatedAt = null,
                Location = "Monterrey",
                Outsourcer = "Quest Labs",
                Priority = "Urgente",
                BirthDate = new DateTime(1970, 12, 5, 0, 0, 0, DateTimeKind.Utc),
            },
            new Sample
            {
                CreatedAt = baseDate.AddDays(2).AddHours(13).AddMinutes(15),
                ReceivedAt = baseDate.AddDays(2).AddHours(14).AddMinutes(5),
                Folio = 20108,
                ClientId = 105,
                PatientId = 2008,
                ExamId = 312,
                ExamName = "TSH",
                ProcessedAt = null,
                ValidatedAt = null,
                Location = "Merida",
                Outsourcer = "Maq Z",
                Priority = "Normal",
                BirthDate = new DateTime(1987, 4, 11, 0, 0, 0, DateTimeKind.Utc),
            },
            new Sample
            {
                CreatedAt = baseDate.AddDays(2).AddHours(15).AddMinutes(50),
                ReceivedAt = baseDate.AddDays(2).AddHours(16).AddMinutes(20),
                Folio = 20109,
                ClientId = 102,
                PatientId = 2009,
                ExamId = 313,
                ExamName = "Acido Urico",
                ProcessedAt = null,
                ValidatedAt = null,
                Location = "Celaya",
                Outsourcer = "LabCorp",
                Priority = "Normal",
                BirthDate = new DateTime(1993, 8, 29, 0, 0, 0, DateTimeKind.Utc),
            },
            new Sample
            {
                CreatedAt = baseDate.AddDays(3).AddHours(7).AddMinutes(20),
                ReceivedAt = baseDate.AddDays(3).AddHours(8).AddMinutes(10),
                Folio = 20110,
                ClientId = 103,
                PatientId = 2010,
                ExamId = 314,
                ExamName = "PSA Total",
                ProcessedAt = baseDate.AddDays(3).AddHours(10).AddMinutes(30),
                ValidatedAt = null,
                Location = "Tijuana",
                Outsourcer = "Maq X",
                Priority = "Stat",
                BirthDate = new DateTime(1960, 3, 16, 0, 0, 0, DateTimeKind.Utc),
            },
            new Sample
            {
                CreatedAt = baseDate.AddDays(3).AddHours(11).AddMinutes(5),
                ReceivedAt = baseDate.AddDays(3).AddHours(12).AddMinutes(0),
                Folio = 20111,
                ClientId = 101,
                PatientId = 2011,
                ExamId = 315,
                ExamName = "Hemoglobina Glicada",
                ProcessedAt = null,
                ValidatedAt = null,
                Location = "Monterrey",
                Outsourcer = "Quest Labs",
                Priority = "Normal",
                BirthDate = new DateTime(1975, 10, 21, 0, 0, 0, DateTimeKind.Utc),
            },
            new Sample
            {
                CreatedAt = baseDate.AddDays(3).AddHours(14).AddMinutes(40),
                ReceivedAt = baseDate.AddDays(3).AddHours(15).AddMinutes(30),
                Folio = 20112,
                ClientId = 104,
                PatientId = 2012,
                ExamId = 316,
                ExamName = "Ferritina",
                ProcessedAt = null,
                ValidatedAt = null,
                Location = "Puebla",
                Outsourcer = "Maq Y",
                Priority = "Urgente",
                BirthDate = new DateTime(1991, 7, 7, 0, 0, 0, DateTimeKind.Utc),
            }
        });

        context.Samples.AddRange(samples);
        await context.SaveChangesAsync();

        var pending = samples.Count(s => s.ValidatedAt == null);
        var completed = samples.Count(s => s.ValidatedAt != null);

        logger.LogInformation("Seeded {Total} samples: {Completed} completed, {Pending} pending",
            samples.Count, completed, pending);
    }

    private static async Task SeedInventoryMovementsAsync(QuimiosDbContext context, ILogger logger)
    {
        if (await context.InventoryMovements.AnyAsync())
        {
            logger.LogInformation("Inventory movements already exist, skipping");
            return;
        }

        logger.LogInformation("Seeding inventory movements...");

        var inventoryItems = await context.InventoryItems
            .Include(i => i.Reagent)
            .Where(i => i.Reagent != null)
            .ToListAsync();

        var movements = new List<InventoryMovement>();
        var random = new Random(42); // Fixed seed for reproducibility

        // Create initial stock IN movements (2-3 weeks ago)
        foreach (var item in inventoryItems)
        {
            var initialReceiptDate = DateTime.UtcNow.AddDays(-random.Next(14, 21));
            movements.Add(new InventoryMovement
            {
                InventoryItemId = item.Id,
                MovementType = "IN",
                Quantity = item.CurrentStock + random.Next(50, 200), // More than current to account for consumption
                Reference = $"Initial Stock Receipt",
                Notes = "Opening inventory balance",
                MovementDate = initialReceiptDate,
                CreatedAt = initialReceiptDate
            });
        }

        // Create some restocking movements (1 week ago)
        var restockItems = inventoryItems.Take(15).ToList();
        foreach (var item in restockItems)
        {
            var restockDate = DateTime.UtcNow.AddDays(-7);
            movements.Add(new InventoryMovement
            {
                InventoryItemId = item.Id,
                MovementType = "IN",
                Quantity = random.Next(100, 300),
                Reference = $"Restock Order #{random.Next(1000, 9999)}",
                Notes = "Regular inventory replenishment",
                MovementDate = restockDate,
                CreatedAt = restockDate
            });
        }

        context.InventoryMovements.AddRange(movements);
        await context.SaveChangesAsync();

        logger.LogInformation("Seeded {Count} inventory movements ({In} IN, {Out} OUT)",
            movements.Count,
            movements.Count(m => m.MovementType == "IN"),
            movements.Count(m => m.MovementType == "OUT"));
    }

    private static async Task SeedConsumptionRecordsAsync(QuimiosDbContext context, ILogger logger)
    {
        if (await context.ConsumptionRecords.AnyAsync())
        {
            logger.LogInformation("Consumption records already exist, skipping");
            return;
        }

        logger.LogInformation("Seeding consumption records...");

        var reagents = await context.Reagents.Where(r => r.IsActive).ToListAsync();
        var inventoryItems = await context.InventoryItems
            .Include(i => i.Reagent)
            .Where(i => i.Reagent != null)
            .ToListAsync();

        var consumptionRecords = new List<ConsumptionRecord>();
        var movements = new List<InventoryMovement>();
        var random = new Random(42);

        // Create consumption for last 7 days
        for (int daysAgo = 7; daysAgo >= 1; daysAgo--)
        {
            var consumptionDate = DateTime.UtcNow.AddDays(-daysAgo).Date;

            // Select 10-20 random reagents per day
            var dailyReagents = reagents.OrderBy(x => random.Next()).Take(random.Next(10, 20)).ToList();

            foreach (var reagent in dailyReagents)
            {
                var inventoryItem = inventoryItems.FirstOrDefault(i => i.ReagentId == reagent.Id);
                if (inventoryItem == null) continue;

                var patients = random.Next(0, 50);
                var repeats = random.Next(0, 10);
                var qc = random.Next(0, 15);
                var calibration = random.Next(0, 2) == 1 ? reagent.CalibrationConsumption : 0;
                var cancellation = random.Next(0, 5);
                var validation = random.Next(0, 8);
                var unidentified = random.Next(0, 3);

                var total = patients + repeats + qc + calibration + cancellation + validation + unidentified;

                if (total == 0) continue;

                var record = new ConsumptionRecord
                {
                    ReagentId = reagent.Id,
                    ConsumptionDate = DateTime.SpecifyKind(consumptionDate, DateTimeKind.Utc),
                    ResearchConsumption = patients,
                    RepeatConsumption = repeats,
                    QCConsumption = qc,
                    CalibrationConsumption = calibration,
                    CancellationConsumption = cancellation,
                    ValidationConsumption = validation,
                    UnidentifiedConsumption = unidentified,
                    ManualConsumption = 0,
                    TotalConsumption = total,
                    CreatedAt = DateTime.SpecifyKind(consumptionDate.AddHours(18), DateTimeKind.Utc) // End of day
                };

                consumptionRecords.Add(record);
            }
        }

        context.ConsumptionRecords.AddRange(consumptionRecords);
        await context.SaveChangesAsync();

        // Create corresponding inventory movements for each consumption
        foreach (var record in consumptionRecords)
        {
            var inventoryItem = inventoryItems.FirstOrDefault(i => i.ReagentId == record.ReagentId);
            if (inventoryItem == null) continue;

            var movement = new InventoryMovement
            {
                InventoryItemId = inventoryItem.Id,
                MovementType = "OUT",
                Quantity = record.TotalConsumption,
                Reference = $"Daily Consumption {record.ConsumptionDate:yyyy-MM-dd}",
                Notes = $"Px:{record.ResearchConsumption} Rep:{record.RepeatConsumption} QC:{record.QCConsumption} Cal:{record.CalibrationConsumption} Canc:{record.CancellationConsumption} Val:{record.ValidationConsumption} Unid:{record.UnidentifiedConsumption}",
                MovementDate = record.ConsumptionDate,
                ConsumptionRecordId = record.Id,
                CreatedAt = record.CreatedAt
            };

            movements.Add(movement);
        }

        context.InventoryMovements.AddRange(movements);
        await context.SaveChangesAsync();

        logger.LogInformation("Seeded {Count} consumption records spanning {Days} days with {Movements} movements",
            consumptionRecords.Count, 7, movements.Count);
    }
}
