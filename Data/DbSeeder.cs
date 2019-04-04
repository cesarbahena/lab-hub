using QuimiosHub.Models;

namespace QuimiosHub.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(QuimiosDbContext context)
    {
        if (context.Samples.Any())
            return;

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
    }
}
