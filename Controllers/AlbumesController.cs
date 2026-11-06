using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicaApi.Data;
using MusicaApi.Models;

namespace MusicaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlbumesController : ControllerBase
{
    private readonly MusicaContext _context;

    public AlbumesController(MusicaContext context) => _context = context;

    // GET api/albumes?artista=&genero=&anio=&q=
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Album>>> GetAlbumes(
        [FromQuery] string? artista,
        [FromQuery] string? genero,
        [FromQuery] int? anio,
        [FromQuery] string? q)
    {
        var query = _context.Albums.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(artista))
            query = query.Where(a => a.Artista.Contains(artista));
        if (!string.IsNullOrWhiteSpace(genero))
            query = query.Where(a => a.Genero == genero);
        if (anio.HasValue)
            query = query.Where(a => a.AnioLanzamiento == anio.Value);
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(a => a.Titulo.Contains(q));

        return Ok(await query.OrderBy(a => a.Artista).ThenBy(a => a.Titulo).ToListAsync());
    }

    // GET api/albumes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Album>> GetAlbum(int id)
    {
        var album = await _context.Albums.FindAsync(id);
        if (album == null) return NotFound();
        return Ok(album);
    }

    // GET api/albumes/5/canciones  (ruta anidada)
    [HttpGet("{id}/canciones")]
    public async Task<ActionResult<IEnumerable<Cancion>>> GetCancionesDeAlbum(int id)
    {
        if (!await _context.Albums.AnyAsync(a => a.Id == id))
            return NotFound();

        var canciones = await _context.Cancions
            .AsNoTracking()
            .Where(c => c.AlbumId == id)
            .OrderBy(c => c.NumeroPista)
            .ToListAsync();

        return Ok(canciones);
    }

    // POST api/albumes
    [HttpPost]
    public async Task<ActionResult<Album>> PostAlbum(Album album)
    {
        if (await _context.Albums.AnyAsync(a => a.Titulo == album.Titulo && a.Artista == album.Artista))
            return Conflict(new { mensaje = "Ya existe un álbum con ese título para este artista." });

        album.Id = 0;
        _context.Albums.Add(album);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAlbum), new { id = album.Id }, album);
    }

    // PUT api/albumes/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAlbum(int id, Album album)
    {
        if (id != album.Id)
            return BadRequest(new { mensaje = "El id de la ruta no coincide con el del cuerpo." });

        if (!await _context.Albums.AnyAsync(a => a.Id == id))
            return NotFound();

        if (await _context.Albums.AnyAsync(a => a.Id != id && a.Titulo == album.Titulo && a.Artista == album.Artista))
            return Conflict(new { mensaje = "Ya existe otro álbum con ese título para este artista." });

        _context.Entry(album).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE api/albumes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAlbum(int id)
    {
        var album = await _context.Albums.FindAsync(id);
        if (album == null) return NotFound();

        if (await _context.Cancions.AnyAsync(c => c.AlbumId == id))
            return Conflict(new { mensaje = "No se puede eliminar un álbum que tiene canciones." });

        _context.Albums.Remove(album);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}