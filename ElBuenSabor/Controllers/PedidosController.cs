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
using ElBuenSabor.Models.Response;

namespace ElBuenSabor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly ElBuenSaborContext _context;
        private readonly IHubContext<NotificacionesAClienteHub> _hubContext;
        private readonly SignalRGroups _signalRGroups;

        const int PENDIENTE = 0;
        const int APROBADO = 1;
        const int LISTO_ENTREGA_LOCAL = 2;
        const int TERMINADO = 3;
        const int PENDIENTE_ENTREGA = 4;
        const int ENTREGADO = 5;
        const int CANCELADO = 6;
        const int COCINANDO = 7;

        public PedidosController(ElBuenSaborContext context, IHubContext<NotificacionesAClienteHub> notificacionesAClienteHub, SignalRGroups signalRGroups)
        {
            _context = context;
            _hubContext = notificacionesAClienteHub;
            _signalRGroups = signalRGroups;
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

            var pedido = await _context.Pedidos
                .Include(p => p.DetallesPedido)
                .ThenInclude(d=>d.Articulo)
                .FirstOrDefaultAsync(c => c.Id == id);

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

            Pedido pedidoPrevio = await _context.Pedidos.AsNoTracking().FirstOrDefaultAsync(x=>x.Id==id);
            int estadoPrevio = pedidoPrevio.Estado;
            int estadoActual = pedido.Estado;


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

            String mensaje="";
            String grupoDestino="";
            PedidoDTO pedidoDTO=new();
            pedidoDTO.Cliente = pedido.Cliente;
            pedidoDTO.Domicilio = pedido.Domicilio;
            pedidoDTO.Estado = pedido.Estado;
            pedidoDTO.Fecha = pedido.Fecha;
            pedidoDTO.HoraEstimadaFin = pedido.HoraEstimadaFin;
            pedidoDTO.Id = pedido.Id;
            pedidoDTO.TipoEnvio = pedido.TipoEnvio;
            pedidoDTO.Total= pedido.Total;


            var cambio = (a: estadoPrevio, b: estadoActual);
                        
            switch (cambio)
            {
                // This case would be handle by FinalizarPedido
                //case (a: PENDIENTE, b: PENDIENTE):
                //    mensaje = "Su pedido esta pendiente de aprobacion";
                //    grupoDestino =  pedido.ClienteID.ToString();
                //    EnviarNotificacionCliente(grupoDestino, mensaje, pedidoDTO);
                //    break;

                case (a: PENDIENTE, b: APROBADO):
                    mensaje = "Su pedido esta Aprobado";
                    grupoDestino = pedido.ClienteID.ToString();
                    EnviarNotificacionCliente(grupoDestino, mensaje, pedidoDTO);

                    mensaje = "Ingresó pedido a cocinar";
                    grupoDestino = _context.Roles.Where(r=>r.Nombre=="Cocinero").FirstOrDefault().Id.ToString() ;
                    EnviarNotificacionRol(grupoDestino, mensaje, pedidoDTO);
                    break;

                case (a: PENDIENTE, b: CANCELADO):
                    mensaje = "Su pedido fue Cancelado";
                    grupoDestino = pedido.ClienteID.ToString();
                    EnviarNotificacionCliente(grupoDestino, mensaje, pedidoDTO);
                    break;

                case (a: CANCELADO, b: PENDIENTE):
                    mensaje = "Su pedido esta pendiente de aprobacion";
                    grupoDestino = pedido.ClienteID.ToString();
                    EnviarNotificacionCliente(grupoDestino, mensaje, pedidoDTO);
                    break;

                case (a: APROBADO, b: PENDIENTE):
                    mensaje = "Disculpe, su pedido ha retrocedido a pendiente de aprobacion";
                    grupoDestino = pedido.ClienteID.ToString();
                    EnviarNotificacionCliente(grupoDestino, mensaje, pedidoDTO);

                    mensaje = "El cajero quitó el pedido de la cocina";
                    grupoDestino = _context.Roles.Where(r => r.Nombre == "Cocinero").FirstOrDefault().Id.ToString();
                    EnviarNotificacionRol(grupoDestino, mensaje, pedidoDTO);
                    break;

                case (a: COCINANDO, b: LISTO_ENTREGA_LOCAL):
                    mensaje = "Su pedido esta Listo para retirar!";
                    grupoDestino = pedido.ClienteID.ToString();
                    EnviarNotificacionCliente(grupoDestino, mensaje, pedidoDTO);

                    mensaje = "El cocinero terminó el pedido. Retira en local";
                    grupoDestino = _context.Roles.Where(r => r.Nombre == "Cajero").FirstOrDefault().Id.ToString();
                    EnviarNotificacionRol(grupoDestino, mensaje, pedidoDTO);
                    break;

                case (a: COCINANDO, b: PENDIENTE_ENTREGA):
                    mensaje = "Su pedido esta está en camino!";
                    grupoDestino = pedido.ClienteID.ToString();
                    EnviarNotificacionCliente(grupoDestino, mensaje, pedidoDTO);

                    mensaje = "El cocinero terminó el pedido. Enviar por delivery";
                    grupoDestino = _context.Roles.Where(r => r.Nombre == "Cajero").FirstOrDefault().Id.ToString();
                    EnviarNotificacionRol(grupoDestino, mensaje, pedidoDTO);
                    break;
                
                case (a: LISTO_ENTREGA_LOCAL, b: ENTREGADO):
                    mensaje = "Esperamos que disfrute su pedido!";
                    grupoDestino = pedido.ClienteID.ToString();
                    EnviarNotificacionCliente(grupoDestino, mensaje, pedidoDTO);
                    break;

                case (a: PENDIENTE_ENTREGA, b: ENTREGADO):
                    mensaje = "Su pedido fue entregado!";
                    grupoDestino = pedido.ClienteID.ToString();
                    EnviarNotificacionCliente(grupoDestino, mensaje, pedidoDTO);
                    break;

                default:
                    break;
            }

            return NoContent();
        }

        private void EnviarNotificacionCliente(String grupoDestino, String mensaje, PedidoDTO pedido) { 
            EnviarNotificacion("C" + grupoDestino, mensaje, pedido);
        }

        private void EnviarNotificacionRol(String grupoDestino, String mensaje, PedidoDTO pedido)
        {
            EnviarNotificacion("R" + grupoDestino, mensaje, pedido);
        }

        private void EnviarNotificacion(String grupoDestino,String mensaje, PedidoDTO pedido)
        {
            _hubContext
            .Clients.Group(grupoDestino)
            .SendAsync("Notificacion", mensaje, pedido);
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

            var pedido = await _context.Pedidos
                .Include(p => p.DetallesPedido)
                .ThenInclude(d => d.Articulo)
                 .Include(p => p.Domicilio)
                        .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

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

            int tiempoEntregaDelivery=0;


            //Si el pedido es a domicili, sumar los 10min extras 
            if (pedido.TipoEnvio == 1)
            {
                tiempoEntregaDelivery = 10;
            }

            long FormulaTE;
            if (_signalRGroups.Cocineros == 0)
            {
                FormulaTE = 9999999;
            }
            else 
            {
                //Comment this to Magni, maybe it's ok just use CantidadDeCocineros from database
                FormulaTE= TECocinaPedidoActual.min + TECocinaPedidosConEstado.min / _signalRGroups.Cocineros + tiempoEntregaDelivery ;
            }

            //Calcular total del pedido (HORRIBLE. EMPROLIJAR)
            foreach (var detalle in pedido.DetallesPedido)
            {
                SQLToJSON ArticuloParaFront = new SQLToJSON();
                var parametros = new Dictionary<String, object>();
                parametros["@pricePoint"] = detalle.Articulo.Id;
                ArticuloParaFront.Agregar("EXECUTE ArticuloParaFront @pricePoint", parametros);
                string artJson = ArticuloParaFront.JSON(); //.Replace("{{", "{").Replace("}}", "}");
                dynamic articulo = JsonConvert.DeserializeObject(artJson);
                float pv = articulo.PrecioVenta;
                pedido.Total += pv * detalle.Cantidad;
            }

            pedido.HoraEstimadaFin = pedido.Fecha.AddMinutes(FormulaTE);
            await PutPedido(id, pedido);

            //-------notificacion
            String mensaje = "";
            String grupoDestino = "";
            PedidoDTO pedidoDTO = new();
            pedidoDTO.Cliente = pedido.Cliente;
            pedidoDTO.Domicilio = pedido.Domicilio;
            pedidoDTO.Estado = pedido.Estado;
            pedidoDTO.Fecha = pedido.Fecha;
            pedidoDTO.HoraEstimadaFin = pedido.HoraEstimadaFin;
            pedidoDTO.Id = pedido.Id;
            pedidoDTO.TipoEnvio = pedido.TipoEnvio;
            pedidoDTO.Total = pedido.Total;


            mensaje = "Ha ingresado un nuevo pedido";
            grupoDestino = _context.Roles.Where(r => r.Nombre == "Cajero").FirstOrDefault().Id.ToString();
            EnviarNotificacionRol(grupoDestino, mensaje, pedidoDTO);

            mensaje = "Su pedido esta pendiente de aprobacion";
            grupoDestino = pedido.ClienteID.ToString();
            EnviarNotificacionCliente(grupoDestino, mensaje, pedidoDTO);

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
