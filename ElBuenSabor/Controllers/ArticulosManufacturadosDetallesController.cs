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
    public class ArticulosManufacturadosDetallesController : ControllerBase
    {
        private readonly ElBuenSaborContext _context;

        public ArticulosManufacturadosDetallesController(ElBuenSaborContext context)
        {
            _context = context;
        }

        // GET: api/ArticulosManufacturadosDetalles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticuloManufacturadoDetalle>>> GetArticulosManufacturadosDetalles()
        {
            return await _context.ArticulosManufacturadosDetalles.ToListAsync();
        }

        // GET: api/ArticulosManufacturadosDetalles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ArticuloManufacturadoDetalle>> GetArticuloManufacturadoDetalle(long id)
        {
            var articuloManufacturadoDetalle = await _context.ArticulosManufacturadosDetalles.FindAsync(id);

            if (articuloManufacturadoDetalle == null)
            {
                return NotFound();
            }

            return articuloManufacturadoDetalle;
        }

        // PUT: api/ArticulosManufacturadosDetalles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticuloManufacturadoDetalle(long id, ArticuloManufacturadoDetalle articuloManufacturadoDetalle)
        {
            if (id != articuloManufacturadoDetalle.Id)
            {
                return BadRequest();
            }

            _context.Entry(articuloManufacturadoDetalle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticuloManufacturadoDetalleExists(id))
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

        // POST: api/ArticulosManufacturadosDetalles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ArticuloManufacturadoDetalle>> PostArticuloManufacturadoDetalle(ArticuloManufacturadoDetalle articuloManufacturadoDetalle)
        {
            _context.ArticulosManufacturadosDetalles.Add(articuloManufacturadoDetalle);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArticuloManufacturadoDetalle", new { id = articuloManufacturadoDetalle.Id }, articuloManufacturadoDetalle);
        }

        // DELETE: api/ArticulosManufacturadosDetalles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticuloManufacturadoDetalle(long id)
        {
            var articuloManufacturadoDetalle = await _context.ArticulosManufacturadosDetalles.FindAsync(id);
            if (articuloManufacturadoDetalle == null)
            {
                return NotFound();
            }

            _context.ArticulosManufacturadosDetalles.Remove(articuloManufacturadoDetalle);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArticuloManufacturadoDetalleExists(long id)
        {
            return _context.ArticulosManufacturadosDetalles.Any(e => e.Id == id);
        }
    }
}
