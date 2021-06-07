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
    public class RubrosArticulosController : ControllerBase
    {
        private readonly ElBuenSaborContext _context;

        public RubrosArticulosController(ElBuenSaborContext context)
        {
            _context = context;
        }

        // GET: api/RubrosArticulos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RubroArticulo>>> GetRubrosArticulos()
        {
            return await _context.RubrosArticulos
                .Include(a => a.Articulos)
                .ToListAsync();
        }

        // GET: api/RubrosArticulos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RubroArticulo>> GetRubroArticulo(long id)
        {
            var rubroArticulo = await _context.RubrosArticulos.FindAsync(id);

            if (rubroArticulo == null)
            {
                return NotFound();
            }

            return rubroArticulo;
        }

        // PUT: api/RubrosArticulos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRubroArticulo(long id, RubroArticulo rubroArticulo)
        {
            if (id != rubroArticulo.Id)
            {
                return BadRequest();
            }

            _context.Entry(rubroArticulo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RubroArticuloExists(id))
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

        // POST: api/RubrosArticulos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RubroArticulo>> PostRubroArticulo(RubroArticulo rubroArticulo)
        {
            _context.RubrosArticulos.Add(rubroArticulo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRubroArticulo", new { id = rubroArticulo.Id }, rubroArticulo);
        }

        // DELETE: api/RubrosArticulos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRubroArticulo(long id)
        {
            var rubroArticulo = await _context.RubrosArticulos.FindAsync(id);
            if (rubroArticulo == null)
            {
                return NotFound();
            }

            _context.RubrosArticulos.Remove(rubroArticulo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RubroArticuloExists(long id)
        {
            return _context.RubrosArticulos.Any(e => e.Id == id);
        }
    }
}
