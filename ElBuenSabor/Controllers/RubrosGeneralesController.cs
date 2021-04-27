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
    public class RubrosGeneralesController : ControllerBase
    {
        private readonly ElBuenSaborContext _context;

        public RubrosGeneralesController(ElBuenSaborContext context)
        {
            _context = context;
        }

        // GET: api/RubrosGenerales
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RubroGeneral>>> GetRubrosGenerales()
        {
            return await _context.RubrosGenerales.ToListAsync();
        }

        // GET: api/RubrosGenerales/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RubroGeneral>> GetRubroGeneral(long id)
        {
            var rubroGeneral = await _context.RubrosGenerales.FindAsync(id);

            if (rubroGeneral == null)
            {
                return NotFound();
            }

            return rubroGeneral;
        }

        // PUT: api/RubrosGenerales/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRubroGeneral(long id, RubroGeneral rubroGeneral)
        {
            if (id != rubroGeneral.Id)
            {
                return BadRequest();
            }

            _context.Entry(rubroGeneral).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RubroGeneralExists(id))
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

        // POST: api/RubrosGenerales
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RubroGeneral>> PostRubroGeneral(RubroGeneral rubroGeneral)
        {
            _context.RubrosGenerales.Add(rubroGeneral);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRubroGeneral", new { id = rubroGeneral.Id }, rubroGeneral);
        }

        // DELETE: api/RubrosGenerales/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRubroGeneral(long id)
        {
            var rubroGeneral = await _context.RubrosGenerales.FindAsync(id);
            if (rubroGeneral == null)
            {
                return NotFound();
            }

            _context.RubrosGenerales.Remove(rubroGeneral);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RubroGeneralExists(long id)
        {
            return _context.RubrosGenerales.Any(e => e.Id == id);
        }
    }
}
