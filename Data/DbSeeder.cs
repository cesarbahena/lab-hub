using QuimiosHub.Models;

namespace QuimiosHub.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(QuimiosDbContext context)
    {
        if (context.Users.Any())
            return;

        var users = new List<User>
        {
            new User
            {
                Username = "jperez",
                FullName = "Juan Perez",
                Email = "jperez@lab.com",
                Role = "Analyst",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                Username = "mgomez",
                FullName = "Maria Gomez",
                Email = "mgomez@lab.com",
                Role = "Supervisor",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

        context.Users.AddRange(users);
        await context.SaveChangesAsync();

        var shifts = new List<Shift>
        {
            new Shift
            {
                Name = "Morning",
                StartTime = new TimeSpan(7, 0, 0),
                EndTime = new TimeSpan(15, 0, 0),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Shift
            {
                Name = "Afternoon",
                StartTime = new TimeSpan(15, 0, 0),
                EndTime = new TimeSpan(23, 0, 0),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

        context.Shifts.AddRange(shifts);
        await context.SaveChangesAsync();

        var samples = new List<Sample>
        {
            new Sample
            {
                FechaGrd = new DateTime(2023, 3, 15, 8, 30, 0),
                FechaRecep = new DateTime(2023, 3, 15, 9, 15, 0),
                FolioGrd = 98765,
                ClienteGrd = 105,
                PacienteGrd = 450,
                EstPerGrd = 789,
                Label1 = "Blood Test",
                FecCapRes = new DateTime(2023, 3, 15, 14, 30, 0),
                FecLibera = new DateTime(2023, 3, 16, 9, 0, 0),
                SucProc = "Lab West",
                Maquilador = "Quest Labs",
                Label3 = "Urgent",
                FecNac = new DateTime(1985, 8, 22),
                CreatedAt = DateTime.UtcNow
            },
            new Sample
            {
                FechaGrd = new DateTime(2023, 2, 2, 9, 30, 0),
                FechaRecep = new DateTime(2023, 2, 2, 10, 30, 0),
                FolioGrd = 12346,
                ClienteGrd = 102,
                PacienteGrd = 203,
                EstPerGrd = 304,
                Label1 = "Another Label 1",
                FecCapRes = new DateTime(2023, 2, 2, 11, 30, 0),
                FecLibera = new DateTime(2023, 2, 2, 12, 30, 0),
                SucProc = "Branch B",
                Maquilador = "Maq Y",
                Label3 = "Another Label 3",
                FecNac = new DateTime(1985, 10, 20),
                CreatedAt = DateTime.UtcNow
            }
        };

        context.Samples.AddRange(samples);
        await context.SaveChangesAsync();

        var inventoryItems = new List<InventoryItem>
        {
            new InventoryItem
            {
                Code = "REA-001",
                Name = "Test Tubes",
                Description = "Standard 10ml test tubes",
                Category = "Containers",
                Unit = "Box",
                CurrentStock = 50,
                MinStock = 20,
                MaxStock = 100,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new InventoryItem
            {
                Code = "REA-002",
                Name = "Reagent A",
                Description = "Chemical reagent for blood analysis",
                Category = "Reagents",
                Unit = "Liter",
                CurrentStock = 15.5m,
                MinStock = 10,
                MaxStock = 50,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

        context.InventoryItems.AddRange(inventoryItems);
        await context.SaveChangesAsync();
    }
}
