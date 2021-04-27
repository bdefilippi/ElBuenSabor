using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElBuenSabor.Models;

namespace ElBuenSabor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticulosInsumoController : ControllerBase
    {
        private readonly ElBuenSaborContext _context;

        public ArticulosInsumoController(ElBuenSaborContext context)
        {
            _context = context;
        }

        // GET: api/ArticulosInsumo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticuloInsumo>>> GetArticulosInsumo()
        {
            return await _context.ArticulosInsumo.ToListAsync();
        }

        // GET: api/ArticulosInsumo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ArticuloInsumo>> GetArticuloInsumo(long id)
        {
            var articuloInsumo = await _context.ArticulosInsumo.FindAsync(id);

            if (articuloInsumo == null)
            {
                return NotFound();
            }

            return articuloInsumo;
        }

        // PUT: api/ArticulosInsumo/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticuloInsumo(long id, ArticuloInsumo articuloInsumo)
        {
            if (id != articuloInsumo.Id)
            {
                return BadRequest();
            }

            _context.Entry(articuloInsumo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticuloInsumoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ArticulosInsumo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ArticuloInsumo>> PostArticuloInsumo(ArticuloInsumo articuloInsumo)
        {
            _context.ArticulosInsumo.Add(articuloInsumo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArticuloInsumo", new { id = articuloInsumo.Id }, articuloInsumo);
        }

        // DELETE: api/ArticulosInsumo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticuloInsumo(long id)
        {
            var articuloInsumo = await _context.ArticulosInsumo.FindAsync(id);
            if (articuloInsumo == null)
            {
                return NotFound();
            }

            _context.ArticulosInsumo.Remove(articuloInsumo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArticuloInsumoExists(long id)
        {
            return _context.ArticulosInsumo.Any(e => e.Id == id);
        }
    }
}
