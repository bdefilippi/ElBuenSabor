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
    public class ArticulosManufacturadosController : ControllerBase
    {
        private readonly ElBuenSaborContext _context;

        public ArticulosManufacturadosController(ElBuenSaborContext context)
        {
            _context = context;
        }

        // GET: api/ArticulosManufacturados
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticuloManufacturado>>> GetArticulosManufacturados()
        {
            return await _context.ArticulosManufacturados.ToListAsync();
        }

        // GET: api/ArticulosManufacturados/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ArticuloManufacturado>> GetArticuloManufacturado(long id)
        {
            var articuloManufacturado = await _context.ArticulosManufacturados.FindAsync(id);

            if (articuloManufacturado == null)
            {
                return NotFound();
            }

            return articuloManufacturado;
        }

        // PUT: api/ArticulosManufacturados/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticuloManufacturado(long id, ArticuloManufacturado articuloManufacturado)
        {
            if (id != articuloManufacturado.Id)
            {
                return BadRequest();
            }

            _context.Entry(articuloManufacturado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticuloManufacturadoExists(id))
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

        // POST: api/ArticulosManufacturados
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ArticuloManufacturado>> PostArticuloManufacturado(ArticuloManufacturado articuloManufacturado)
        {
            _context.ArticulosManufacturados.Add(articuloManufacturado);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetArticuloManufacturado", new { id = articuloManufacturado.Id }, articuloManufacturado);
            return CreatedAtAction(nameof(GetArticuloManufacturado), new { id = articuloManufacturado.Id }, articuloManufacturado);
        }

        // DELETE: api/ArticulosManufacturados/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticuloManufacturado(long id)
        {
            var articuloManufacturado = await _context.ArticulosManufacturados.FindAsync(id);
            if (articuloManufacturado == null)
            {
                return NotFound();
            }

            _context.ArticulosManufacturados.Remove(articuloManufacturado);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArticuloManufacturadoExists(long id)
        {
            return _context.ArticulosManufacturados.Any(e => e.Id == id);
        }
    }
}
