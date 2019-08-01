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
            await SeedSamplesFromCsvAsync(context, logger);

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

    private static async Task SeedSamplesFromCsvAsync(QuimiosDbContext context, ILogger logger)
    {
        if (await context.Samples.AnyAsync())
        {
            logger.LogInformation("Samples already exist, skipping");
            return;
        }

        logger.LogInformation("Seeding samples from Muestras.csv structure...");

        // Simulate data that would come from ETL scraper
        var samples = new List<Sample>
        {
            new Sample
            {
                FechaGrd = new DateTime(2023, 2, 1, 10, 0, 0, DateTimeKind.Utc),
                FechaRecep = new DateTime(2023, 2, 1, 11, 0, 0, DateTimeKind.Utc),
                FolioGrd = 12345,
                ClienteGrd = 101,
                PacienteGrd = 202,
                EstPerGrd = 303,
                Label1 = "Some Label 1",
                FecCapRes = new DateTime(2023, 2, 1, 12, 0, 0, DateTimeKind.Utc),
                FecLibera = new DateTime(2023, 2, 1, 13, 0, 0, DateTimeKind.Utc),
                SucProc = "Branch A",
                Maquilador = "Maq X",
                Label3 = "Some Label 3",
                FecNac = new DateTime(1990, 5, 15, 0, 0, 0, DateTimeKind.Utc),
                CreatedAt = DateTime.UtcNow
            },
            new Sample
            {
                FechaGrd = new DateTime(2023, 2, 2, 9, 30, 0, DateTimeKind.Utc),
                FechaRecep = new DateTime(2023, 2, 2, 10, 30, 0, DateTimeKind.Utc),
                FolioGrd = 12346,
                ClienteGrd = 102,
                PacienteGrd = 203,
                EstPerGrd = 304,
                Label1 = "Another Label 1",
                FecCapRes = new DateTime(2023, 2, 2, 11, 30, 0, DateTimeKind.Utc),
                FecLibera = new DateTime(2023, 2, 2, 12, 30, 0, DateTimeKind.Utc),
                SucProc = "Branch B",
                Maquilador = "Maq Y",
                Label3 = "Another Label 3",
                FecNac = new DateTime(1985, 10, 20, 0, 0, 0, DateTimeKind.Utc),
                CreatedAt = DateTime.UtcNow
            },
            // PENDING SAMPLES (FecLibera = null)
            new Sample
            {
                FechaGrd = DateTime.UtcNow.AddDays(-2),
                FechaRecep = DateTime.UtcNow.AddDays(-2).AddHours(1),
                FolioGrd = 12347,
                ClienteGrd = 101,
                PacienteGrd = 204,
                EstPerGrd = 305,
                Label1 = "Pending Sample 1",
                FecCapRes = DateTime.UtcNow.AddDays(-1),
                FecLibera = null,  // PENDING
                SucProc = "Branch A",
                Maquilador = "Maq X",
                Label3 = "Pending",
                FecNac = new DateTime(1992, 3, 10, 0, 0, 0, DateTimeKind.Utc),
                CreatedAt = DateTime.UtcNow
            },
            new Sample
            {
                FechaGrd = DateTime.UtcNow.AddDays(-1),
                FechaRecep = DateTime.UtcNow.AddDays(-1).AddHours(2),
                FolioGrd = 12348,
                ClienteGrd = 102,
                PacienteGrd = 205,
                EstPerGrd = 306,
                Label1 = "Pending Sample 2",
                FecCapRes = null,
                FecLibera = null,  // PENDING
                SucProc = "Branch B",
                Maquilador = "Maq Y",
                Label3 = "Pending",
                FecNac = new DateTime(1988, 7, 25, 0, 0, 0, DateTimeKind.Utc),
                CreatedAt = DateTime.UtcNow
            }
        };

        context.Samples.AddRange(samples);
        await context.SaveChangesAsync();

        var pending = samples.Count(s => s.FecLibera == null);
        var completed = samples.Count(s => s.FecLibera != null);

        logger.LogInformation("Seeded {Total} samples: {Completed} completed, {Pending} pending",
            samples.Count, completed, pending);
    }
}
