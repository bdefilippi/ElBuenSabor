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
    public class PreciosVentaArticulosController : ControllerBase
    {
        private readonly ElBuenSaborContext _context;

        public PreciosVentaArticulosController(ElBuenSaborContext context)
        {
            _context = context;
        }

        // GET: api/PreciosVentaArticulos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrecioVentaArticulo>>> GetPreciosVentaArticulos()
        {
            return await _context.PreciosVentaArticulos.ToListAsync();
        }

        // GET: api/PreciosVentaArticulos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PrecioVentaArticulo>> GetPrecioVentaArticulo(long id)
        {
            var precioVentaArticulo = await _context.PreciosVentaArticulos.FindAsync(id);

            if (precioVentaArticulo == null)
            {
                return NotFound();
            }

            return precioVentaArticulo;
        }

        // PUT: api/PreciosVentaArticulos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPrecioVentaArticulo(long id, PrecioVentaArticulo precioVentaArticulo)
        {
            if (id != precioVentaArticulo.Id)
            {
                return BadRequest();
            }

            _context.Entry(precioVentaArticulo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrecioVentaArticuloExists(id))
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

        // POST: api/PreciosVentaArticulos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PrecioVentaArticulo>> PostPrecioVentaArticulo(PrecioVentaArticulo precioVentaArticulo)
        {
            _context.PreciosVentaArticulos.Add(precioVentaArticulo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPrecioVentaArticulo", new { id = precioVentaArticulo.Id }, precioVentaArticulo);
        }

        // DELETE: api/PreciosVentaArticulos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrecioVentaArticulo(long id)
        {
            var precioVentaArticulo = await _context.PreciosVentaArticulos.FindAsync(id);
            if (precioVentaArticulo == null)
            {
                return NotFound();
            }

            _context.PreciosVentaArticulos.Remove(precioVentaArticulo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PrecioVentaArticuloExists(long id)
        {
            return _context.PreciosVentaArticulos.Any(e => e.Id == id);
        }
    }
}
