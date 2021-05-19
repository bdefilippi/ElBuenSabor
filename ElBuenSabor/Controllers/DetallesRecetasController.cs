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
    public class DetallesRecetasController : ControllerBase
    {
        private readonly ElBuenSaborContext _context;

        public DetallesRecetasController(ElBuenSaborContext context)
        {
            _context = context;
        }

        // GET: api/DetallesRecetas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetalleReceta>>> GetDetallesRecetas()
        {
            return await _context.DetallesRecetas.ToListAsync();
        }

        // GET: api/DetallesRecetas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DetalleReceta>> GetDetalleReceta(long id)
        {
            var detalleReceta = await _context.DetallesRecetas.FindAsync(id);

            if (detalleReceta == null)
            {
                return NotFound();
            }

            return detalleReceta;
        }

        // PUT: api/DetallesRecetas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDetalleReceta(long id, DetalleReceta detalleReceta)
        {
            if (id != detalleReceta.Id)
            {
                return BadRequest();
            }

            _context.Entry(detalleReceta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetalleRecetaExists(id))
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

        // POST: api/DetallesRecetas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DetalleReceta>> PostDetalleReceta(DetalleReceta detalleReceta)
        {
            _context.DetallesRecetas.Add(detalleReceta);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDetalleReceta", new { id = detalleReceta.Id }, detalleReceta);
        }

        // DELETE: api/DetallesRecetas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetalleReceta(long id)
        {
            var detalleReceta = await _context.DetallesRecetas.FindAsync(id);
            if (detalleReceta == null)
            {
                return NotFound();
            }

            _context.DetallesRecetas.Remove(detalleReceta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DetalleRecetaExists(long id)
        {
            return _context.DetallesRecetas.Any(e => e.Id == id);
        }
    }
}
