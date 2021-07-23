using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElBuenSabor.Models;
using MercadoPago.Config;
using MercadoPago.Client.Preference;
using MercadoPago.Resource.Preference;
using ElBuenSabor.Hubs;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace ElBuenSabor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly ElBuenSaborContext _context;
        private readonly IHubContext<NotificacionesAClienteHub> _hubContext;

        public PedidosController(ElBuenSaborContext context, IHubContext<NotificacionesAClienteHub> notificacionesAClienteHub)
        {
            _context = context;
            _hubContext = notificacionesAClienteHub;
        }

        // GET: api/Pedidos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidos()
        {
            return await _context.Pedidos.ToListAsync();
        }

        // GET: api/Pedidos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> GetPedido(long id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);

            if (pedido == null)
            {
                return NotFound();
            }

            return pedido;
        }

        // PUT: api/Pedidos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPedido(long id, Pedido pedido)
        {
            if (id != pedido.Id)
            {
                return BadRequest();
            }

            _context.Entry(pedido).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PedidoExists(id))
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

        // POST: api/Pedidos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pedido>> PostPedido(Pedido pedido)
        {
                 
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPedido", new { id = pedido.Id }, pedido);
        }


        // POST: api/Pedidos/Finalizar/5
        [HttpPost("Finalizar/{id}")]
        public async Task<ActionResult<Pedido>> FinalizarPedido(long id)
        {
            var pedido = GetPedido(id).Result.Value;

            /*
             Finalizada la carga del pedido el sistema le informara al cliente 
             cuanto es el tiempo estimado para el retiro o entrega de su pedido, 
             este tiempo surgirá de la siguiente formula:
                Σ Sumatoria del tiempo estimado de los artículos manufacturados 
                  solicitados por el cliente en el pedido actual
                +
                Σ Sumatoria del tiempo estimado de los artículos manufacturados 
                  que se encuentran en la cocina / cantidad cocineros
                +
                10 Minutos de entrega por Delivery, solo si corresponde.
             */
            SQLToJSON TiempoEstimadoCocinaPedidoActual = new SQLToJSON();
            SQLToJSON TiempoEstimadoCocinaPedidosConEstado = new SQLToJSON();

            var parametroIdPedidoActual = new Dictionary<String, object>();
            parametroIdPedidoActual["@IdPedido"] = pedido.Id;
            TiempoEstimadoCocinaPedidoActual.Agregar("EXECUTE TiempoEstimadoCocinaPedidoActual @IdPedido", parametroIdPedidoActual);
            TiempoEstimadoCocinaPedidoActual.JSON();

            var parametroEstadoActual = new Dictionary<String, object>();
            parametroEstadoActual["@estado"] = 0;
            TiempoEstimadoCocinaPedidosConEstado.Agregar("EXECUTE TiempoEstimadoCocinaPedidosConEstado @estado", parametroEstadoActual);
            TiempoEstimadoCocinaPedidosConEstado.JSON();

            
            //agrega la fecha de hoy al pedido
            pedido.Fecha = DateTime.Now;

            //Agrega fecha estimada de finalizacion
            dynamic TECocinaPedidoActual = JsonConvert.DeserializeObject(TiempoEstimadoCocinaPedidoActual.JSON());
            dynamic TECocinaPedidosConEstado = JsonConvert.DeserializeObject(TiempoEstimadoCocinaPedidosConEstado.JSON());

            //FALTA DIVIDIR POR LA CANTIDAD DE COCINEROS
            long FormulaTE = TECocinaPedidoActual.min + TECocinaPedidosConEstado.min;
            pedido.HoraEstimadaFin = pedido.Fecha.AddMinutes(FormulaTE);
            await PutPedido(id, pedido);

            //-------notificacion
            await _hubContext
            .Clients.Group(pedido.ClienteID.ToString())
            //.All
            .SendAsync("NotificacionPedidoRecibido", "Su pedido esta pendiente de aprobacion", pedido);
            //-------fin notificacion

            return (pedido);
        }

        // POST: api/Pedidos/{total}
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{total}")]
        public async Task<ActionResult<Preference>> MercadoPago(decimal total)
        {

            MercadoPagoConfig.AccessToken = "TEST-5059945658019779-070913-a1924cb562898b6ed9191db0f41badf6-155784029";

            var request = new PreferenceRequest
            {
                Items = new List<PreferenceItemRequest>
    {
        new PreferenceItemRequest
        {
            Title = "Carrito",
            Quantity = 1,
            CurrencyId = "ARS",
            UnitPrice = total,
        },
    },
            };

            // Crea la preferencia usando el client
            var client = new PreferenceClient();
            Preference preference = await client.CreateAsync(request);
            
            return preference;
        }


        // DELETE: api/Pedidos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePedido(long id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }

            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PedidoExists(long id)
        {
            return _context.Pedidos.Any(e => e.Id == id);
        } 



    }
}
