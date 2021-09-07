using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElBuenSabor.Models;
using Newtonsoft.Json;
using System.Net.Http;

namespace ElBuenSabor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MercadoPagoDatosController : ControllerBase
    {
        private readonly ElBuenSaborContext _context;

        public MercadoPagoDatosController(ElBuenSaborContext context)
        {
            _context = context;
        }

        // GET: api/MercadoPagoDatos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MercadoPagoDatos>>> GetMercadoPagoDatos()
        {
            return await _context.MercadoPagoDatos.ToListAsync();
        }

        // GET: api/MercadoPagoDatos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MercadoPagoDatos>> GetMercadoPagoDatos(long id)
        {
            var mercadoPagoDatos = await _context.MercadoPagoDatos.FindAsync(id);

            if (mercadoPagoDatos == null)
            {
                return NotFound();
            }

            return mercadoPagoDatos;
        }

        // PUT: api/MercadoPagoDatos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMercadoPagoDatos(long id, MercadoPagoDatos mercadoPagoDatos)
        {
            if (id != mercadoPagoDatos.Id)
            {
                return BadRequest();
            }

            _context.Entry(mercadoPagoDatos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MercadoPagoDatosExists(id))
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

        // POST: api/MercadoPagoDatos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<StatusCodeResult> PostMercadoPagoEstado([FromQuery] string topic,long id)
        {

            String urlApi = "https://api.mercadopago.com//v1/payments/" + id;
            
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(urlApi))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    dynamic reservationList = JsonConvert.DeserializeObject<dynamic>(apiResponse);
                }
            }

            MercadoPagoDatos mercadoPagoDatos = new();
            _context.MercadoPagoDatos.Add(mercadoPagoDatos);
            await _context.SaveChangesAsync();

            return StatusCode(200);
        }


        // POST: api/MercadoPagoDatos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MercadoPagoDatos>> PostMercadoPagoDatos(MercadoPagoDatos mercadoPagoDatos)
        {
            _context.MercadoPagoDatos.Add(mercadoPagoDatos);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMercadoPagoDatos", new { id = mercadoPagoDatos.Id }, mercadoPagoDatos);
        }

        // DELETE: api/MercadoPagoDatos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMercadoPagoDatos(long id)
        {
            var mercadoPagoDatos = await _context.MercadoPagoDatos.FindAsync(id);
            if (mercadoPagoDatos == null)
            {
                return NotFound();
            }

            _context.MercadoPagoDatos.Remove(mercadoPagoDatos);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MercadoPagoDatosExists(long id)
        {
            return _context.MercadoPagoDatos.Any(e => e.Id == id);
        }
    }
}
