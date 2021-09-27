using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElBuenSabor.Models;
using Microsoft.AspNetCore.Authorization;

namespace ElBuenSabor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrador,Cajero")]

    public class EgresosArticulosController : ControllerBase
    {
        private readonly ElBuenSaborContext _context;

        public EgresosArticulosController(ElBuenSaborContext context)
        {
            _context = context;
        }

        // GET: api/EgresosArticulos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EgresoArticulo>>> GetEgresosArticulos()
        {
            return await _context.EgresosArticulos.ToListAsync();
        }

        // GET: api/EgresosArticulos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EgresoArticulo>> GetEgresoArticulo(long id)
        {
            var egresoArticulo = await _context.EgresosArticulos.FindAsync(id);

            if (egresoArticulo == null)
            {
                return NotFound();
            }

            return egresoArticulo;
        }

        // PUT: api/EgresosArticulos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEgresoArticulo(long id, EgresoArticulo egresoArticulo)
        {
            if (id != egresoArticulo.Id)
            {
                return BadRequest();
            }

            _context.Entry(egresoArticulo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EgresoArticuloExists(id))
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

        // POST: api/EgresosArticulos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EgresoArticulo>> PostEgresoArticulo(EgresoArticulo egresoArticulo)
        {
            _context.EgresosArticulos.Add(egresoArticulo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEgresoArticulo", new { id = egresoArticulo.Id }, egresoArticulo);
        }

        // DELETE: api/EgresosArticulos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEgresoArticulo(long id)
        {
            var egresoArticulo = await _context.EgresosArticulos.FindAsync(id);
            if (egresoArticulo == null)
            {
                return NotFound();
            }

            _context.EgresosArticulos.Remove(egresoArticulo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EgresoArticuloExists(long id)
        {
            return _context.EgresosArticulos.Any(e => e.Id == id);
        }
    }
}
