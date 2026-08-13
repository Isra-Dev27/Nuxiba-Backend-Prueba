using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuxibaApi.Data;
using System.Globalization;
using System.Text;

namespace NuxibaApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ReportsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("logins-csv")]
    public async Task<IActionResult> GetLoginsCsv()
    {

        var users = await _context.Users
            .Include(u => u.Area)
            .Include(u => u.LoginRecords)
            .ToListAsync();

        var records = new List<CsvRow>();


        foreach (var user in users)
        {
            var orderedLogins = user.LoginRecords.OrderBy(l => l.fecha).ToList();
            double totalHours = 0;
            
            DateTime? currentLogin = null;

            foreach (var log in orderedLogins)
            {
                if (log.TipoMov == 1)
                {
                    currentLogin = log.fecha;
                }
                else if (log.TipoMov == 0 && currentLogin.HasValue)
                {
                    totalHours += (log.fecha - currentLogin.Value).TotalHours;
                    currentLogin = null;
                }
            }

            records.Add(new CsvRow
            {
                NombreUsuario = user.Login ?? "",
                NombreCompleto = $"{user.Nombres} {user.ApellidoPaterno} {user.ApellidoMaterno}".Trim(),
                Area = user.Area?.AreaName ?? "Sin Área",
                HorasTrabajadas = Math.Round(totalHours, 2)
            });
        }


        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ","
        };

        using var memoryStream = new MemoryStream();
        using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
        using (var csv = new CsvWriter(writer, config))
        {
            await csv.WriteRecordsAsync(records);
        }

        return File(memoryStream.ToArray(), "text/csv", "logins_report.csv");
    }

    public class CsvRow
    {
        public string NombreUsuario { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public double HorasTrabajadas { get; set; }
    }
}
