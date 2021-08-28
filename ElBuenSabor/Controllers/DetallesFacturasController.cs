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
    public class DetallesFacturasController : ControllerBase
    {
        private readonly ElBuenSaborContext _context;

        public DetallesFacturasController(ElBuenSaborContext context)
        {
            _context = context;
        }

        // GET: api/DetallesFacturas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetalleFactura>>> GetDetallesFacturas()
        {
            return await _context.DetallesFacturas.ToListAsync();
        }

        // GET: api/DetallesFacturas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DetalleFactura>> GetDetalleFactura(long id)
        {
            var detalleFactura = await _context.DetallesFacturas.FindAsync(id);

            if (detalleFactura == null)
            {
                return NotFound();
            }

            return detalleFactura;
        }

        // PUT: api/DetallesFacturas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDetalleFactura(long id, DetalleFactura detalleFactura)
        {
            if (id != detalleFactura.Id)
            {
                return BadRequest();
            }

            _context.Entry(detalleFactura).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetalleFacturaExists(id))
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

        // POST: api/DetallesFacturas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DetalleFactura>> PostDetalleFactura(DetalleFactura detalleFactura)
        {
            _context.DetallesFacturas.Add(detalleFactura);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction("GetDetalleFactura", new { id = detalleFactura.Id }, detalleFactura);
        }



        // DELETE: api/DetallesFacturas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetalleFactura(long id)
        {
            var detalleFactura = await _context.DetallesFacturas.FindAsync(id);
            if (detalleFactura == null)
            {
                return NotFound();
            }

            _context.DetallesFacturas.Remove(detalleFactura);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DetalleFacturaExists(long id)
        {
            return _context.DetallesFacturas.Any(e => e.Id == id);
        }
    }
}
