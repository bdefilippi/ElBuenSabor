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
    public class ConfiguracionesController : ControllerBase
    {
        private readonly ElBuenSaborContext _context;

        public ConfiguracionesController(ElBuenSaborContext context)
        {
            _context = context;
        }

        // GET: api/Configuraciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Configuracion>>> GetConfiguraciones()
        {
            return await _context.Configuraciones.ToListAsync();
        }

        // GET: api/Configuraciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Configuracion>> GetConfiguracion(long id)
        {
            var configuracion = await _context.Configuraciones.FindAsync(id);

            if (configuracion == null)
            {
                return NotFound();
            }

            return configuracion;
        }

        // PUT: api/Configuraciones/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConfiguracion(long id, Configuracion configuracion)
        {
            if (id != configuracion.Id)
            {
                return BadRequest();
            }

            _context.Entry(configuracion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConfiguracionExists(id))
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

        // POST: api/Configuraciones
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Configuracion>> PostConfiguracion(Configuracion configuracion)
        {
            _context.Configuraciones.Add(configuracion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConfiguracion", new { id = configuracion.Id }, configuracion);
        }

        // DELETE: api/Configuraciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfiguracion(long id)
        {
            var configuracion = await _context.Configuraciones.FindAsync(id);
            if (configuracion == null)
            {
                return NotFound();
            }

            _context.Configuraciones.Remove(configuracion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConfiguracionExists(long id)
        {
            return _context.Configuraciones.Any(e => e.Id == id);
        }
    }
}
