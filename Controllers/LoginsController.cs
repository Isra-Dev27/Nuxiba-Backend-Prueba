using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuxibaApi.Data;
using NuxibaApi.Models;

namespace NuxibaApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginsController : ControllerBase
{
    private readonly AppDbContext _context;

    public LoginsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var logins = await _context.LoginRecords.ToListAsync();
        return Ok(logins);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] LoginRecord record)
    {

        if (record.fecha == default(DateTime) || record.fecha < new DateTime(2000, 1, 1))
        {
            return BadRequest("La fecha no es válida.");
        }


        var userExists = await _context.Users.AnyAsync(u => u.User_id == record.User_id);
        if (!userExists)
        {
            return BadRequest($"El User_id {record.User_id} no existe en la tabla ccUsers.");
        }


        var lastRecord = await _context.LoginRecords
            .Where(l => l.User_id == record.User_id)
            .OrderByDescending(l => l.fecha)
            .FirstOrDefaultAsync();

        if (lastRecord != null)
        {
            if (record.TipoMov == 1 && lastRecord.TipoMov == 1)
            {
                return BadRequest("No se puede registrar un login sin un logout anterior.");
            }
            
            if (record.TipoMov == 0 && lastRecord.TipoMov == 0)
            {
                return BadRequest("No se puede registrar un logout sin un login anterior.");
            }
            
            if (record.fecha <= lastRecord.fecha)
            {
                return BadRequest("La fecha del nuevo registro debe ser mayor al último registro del usuario.");
            }
        }
        else
        {
            if (record.TipoMov == 0)
            {
                return BadRequest("No se puede registrar un logout inicial sin un login.");
            }
        }

        _context.LoginRecords.Add(record);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAll), new { id = record.Id }, record);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] LoginRecord record)
    {
        if (id != record.Id)
            return BadRequest("El ID de la ruta no coincide con el del registro.");

        var existing = await _context.LoginRecords.FindAsync(id);
        if (existing == null)
            return NotFound("Registro no encontrado.");

        existing.Extension = record.Extension;
        existing.TipoMov = record.TipoMov;
        existing.fecha = record.fecha;


        await _context.SaveChangesAsync();
        return Ok(existing);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var record = await _context.LoginRecords.FindAsync(id);
        if (record == null)
            return NotFound("Registro no encontrado.");

        _context.LoginRecords.Remove(record);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
