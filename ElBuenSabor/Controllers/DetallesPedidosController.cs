using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElBuenSabor.Models;
using Newtonsoft.Json;

namespace ElBuenSabor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetallesPedidosController : ControllerBase
    {
        private readonly ElBuenSaborContext _context;

        public DetallesPedidosController(ElBuenSaborContext context)
        {
            _context = context;
        }

        // GET: api/DetallesPedidos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetallePedido>>> GetDetallesPedidos()
        {
            return await _context.DetallesPedidos.ToListAsync();
        }

        // GET: api/DetallesPedidos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DetallePedido>> GetDetallePedido(long id)
        {
            var detallePedido = await _context.DetallesPedidos.FindAsync(id);

            if (detallePedido == null)
            {
                return NotFound();
            }

            return detallePedido;
        }

        // PUT: api/DetallesPedidos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDetallePedido(long id, DetallePedido detallePedido)
        {
            if (id != detallePedido.Id)
            {
                return BadRequest();
            }

            _context.Entry(detallePedido).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetallePedidoExists(id))
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

        // POST: api/DetallesPedidos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DetallePedido>> PostDetallePedido(DetallePedido detallePedido)
        {
            //Llena el campo subtotal. No lo trae desde el front por seguridad
            dynamic articuloParaFront = JsonConvert.DeserializeObject(ArticulosController.GetArticuloParaFrontStatic(detallePedido.ArticuloID));
            detallePedido.Subtotal = articuloParaFront.PrecioVenta * detallePedido.Cantidad;

            _context.DetallesPedidos.Add(detallePedido);
            await _context.SaveChangesAsync();

            
            return CreatedAtAction("GetDetallePedido", new { id = detallePedido.Id }, detallePedido);
        }

        // DELETE: api/DetallesPedidos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetallePedido(long id)
        {
            var detallePedido = await _context.DetallesPedidos.FindAsync(id);
            if (detallePedido == null)
            {
                return NotFound();
            }

            _context.DetallesPedidos.Remove(detallePedido);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DetallePedidoExists(long id)
        {
            return _context.DetallesPedidos.Any(e => e.Id == id);
        }
    }
}
