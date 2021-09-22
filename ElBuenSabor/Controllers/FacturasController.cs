using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElBuenSabor.Models;
using System.Net.Http.Headers;
using System.IO;
using System.Net.Http;
using System.Net;

namespace ElBuenSabor.Controllers
{




    [Route("api/[controller]")]
    [ApiController]
    public class FacturasController : ControllerBase
    {
        private readonly ElBuenSaborContext _context;

        public FacturasController(ElBuenSaborContext context)
        {
            _context = context;
        }

        // GET: api/Facturas/PDF/5
        [HttpGet("PDF/{pedidoId}")]
        public async Task<ActionResult> DownloadFile(long pedidoId)
        {
            Factura factura = await _context.Facturas
            .Include(a => a.Pedido)
            .ThenInclude(a => a.DetallesPedido)
            .ThenInclude(a => a.Articulo)
            .Include(a => a.Pedido.Domicilio)
            .Include(a => a.Pedido.Cliente)
            .AsNoTracking()
            .Where(a=>a.PedidoID== pedidoId)
            .FirstOrDefaultAsync();
            
            string workingDirectory = Environment.CurrentDirectory + "\\wwwroot\\PDF\\";
            String fileName = @"F-" + Convert.ToString(factura.Numero) + " - " + factura.Pedido.Cliente.Apellido + " " + factura.Pedido.Cliente.Nombre + ".pdf";
            String filePath = workingDirectory + fileName;

            var bytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(bytes, "text/plain", Path.GetFileName(filePath));
        }

        // GET: api/Facturas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Factura>>> GetFacturas()
        {
            return await _context.Facturas.ToListAsync();
        }

        // GET: api/Facturas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Factura>> GetFactura(long id)
        {
            var factura = await _context.Facturas.FindAsync(id);

            if (factura == null)
            {
                return NotFound();
            }

            return factura;
        }


        // GET: api/Facturas/Pedido/5
        [HttpGet("Pedido/{id}")]
        public async Task<ActionResult<Factura>> GetFacturaDePedido(long id)
        {
            var factura = await _context.Facturas.Where(f=>f.PedidoID==id).FirstOrDefaultAsync();

            if (factura == null)
            {
                return NotFound();
            }

            return factura;
        }

        // GET: api/Facturas/PedidoExiste/5
        [HttpGet("PedidoExiste/{id}")]
        public async Task<ActionResult<Boolean>> GetFacturaDePedidoExiste(long id)
        {
            return await _context.Facturas.Where(f => f.PedidoID == id).AnyAsync();

        }


        // PUT: api/Facturas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFactura(long id, Factura factura)
        {
            if (id != factura.Id)
            {
                return BadRequest();
            }

            _context.Entry(factura).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FacturaExists(id))
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

        // POST: api/Facturas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Factura>> PostFactura(Factura factura)
        {
            _context.Facturas.Add(factura);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFactura", new { id = factura.Id }, factura);
        }

        // DELETE: api/Facturas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFactura(long id)
        {
            var factura = await _context.Facturas.FindAsync(id);
            if (factura == null)
            {
                return NotFound();
            }

            _context.Facturas.Remove(factura);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FacturaExists(long id)
        {
            return _context.Facturas.Any(e => e.Id == id);
        }
    }
}
