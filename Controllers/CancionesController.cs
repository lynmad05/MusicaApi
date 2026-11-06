using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicaApi.Data;
using MusicaApi.Models;

namespace MusicaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CancionesController : ControllerBase
{
    private readonly MusicaContext _context;

    public CancionesController(MusicaContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cancion>>> GetCanciones(
        [FromQuery] int? albumId,
        [FromQuery] string? titulo,
        [FromQuery] int? duracionMax)
    {
        var query = _context.Cancions.AsNoTracking().Include(c => c.Album).AsQueryable();

        if (albumId.HasValue)
            query = query.Where(c => c.AlbumId == albumId.Value);
        if (!string.IsNullOrWhiteSpace(titulo))
            query = query.Where(c => c.Titulo.Contains(titulo));
        if (duracionMax.HasValue)
            query = query.Where(c => c.DuracionSegundos <= duracionMax.Value);

        return Ok(await query.OrderBy(c => c.AlbumId).ThenBy(c => c.NumeroPista).ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Cancion>> GetCancion(int id)
    {
        var cancion = await _context.Cancions
            .AsNoTracking()
            .Include(c => c.Album)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cancion == null) return NotFound();
        return Ok(cancion);
    }

    [HttpPost]
    public async Task<ActionResult<Cancion>> PostCancion(Cancion cancion)
    {
        if (!await _context.Albums.AnyAsync(a => a.Id == cancion.AlbumId))
        {
            ModelState.AddModelError(nameof(Cancion.AlbumId), "El álbum indicado no existe.");
            return ValidationProblem(ModelState);
        }

        if (await _context.Cancions.AnyAsync(c => c.AlbumId == cancion.AlbumId && c.NumeroPista == cancion.NumeroPista))
            return Conflict(new { mensaje = "Ya existe una canción con ese número de pista en el álbum." });

        cancion.Id = 0;
        _context.Cancions.Add(cancion);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCancion), new { id = cancion.Id }, cancion);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutCancion(int id, Cancion cancion)
    {
        if (id != cancion.Id)
            return BadRequest(new { mensaje = "El id de la ruta no coincide con el del cuerpo." });

        if (!await _context.Cancions.AnyAsync(c => c.Id == id))
            return NotFound();

        if (!await _context.Albums.AnyAsync(a => a.Id == cancion.AlbumId))
        {
            ModelState.AddModelError(nameof(Cancion.AlbumId), "El álbum indicado no existe.");
            return ValidationProblem(ModelState);
        }

        if (await _context.Cancions.AnyAsync(c => c.Id != id && c.AlbumId == cancion.AlbumId && c.NumeroPista == cancion.NumeroPista))
            return Conflict(new { mensaje = "Ya existe otra canción con ese número de pista en el álbum." });

        _context.Entry(cancion).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCancion(int id)
    {
        var cancion = await _context.Cancions.FindAsync(id);
        if (cancion == null) return NotFound();

        _context.Cancions.Remove(cancion);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}