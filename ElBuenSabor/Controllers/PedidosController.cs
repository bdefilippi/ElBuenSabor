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
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Net.Mail;
using SelectPdf;
using ElBuenSabor.Tools;
using System.IO;

namespace ElBuenSabor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly ElBuenSaborContext _context;
        
        private readonly IHubContext<NotificacionesAClienteHub> _hubContext;

        const int PAGO_PENDIENTE_MP = -1;
        const int PENDIENTE = 0;
        const int APROBADO = 1;
        const int LISTO_ENTREGA_LOCAL = 2;
        const int TERMINADO = 3;
        const int PENDIENTE_ENTREGA = 4;
        const int ENTREGADO = 5;
        const int CANCELADO = 6;
        const int COCINANDO = 7;

        const int LOCAL = 0;
        const int DOMICILIO = 1;
    

        public PedidosController(ElBuenSaborContext context, IHubContext<NotificacionesAClienteHub> notificacionesAClienteHub)
        {
            _context = context;
            _hubContext = notificacionesAClienteHub;
        }

        // GET: api/Pedidos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidos()
        {
            var pedidos = await _context.Pedidos.Include(d=>d.DetallesPedido).ToListAsync();
            foreach (Pedido pedido in pedidos)
            {
                pedido.Total = PedidoTotalCalcular(pedido);
            }

            return pedidos;
        }

        // GET: api/Pedidos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> GetPedido(long id)
        {

            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.DetallesPedido)
                .ThenInclude(d=>d.Articulo)
                .FirstOrDefaultAsync(c => c.Id == id);

            PedidoTotalModificar(ref pedido);

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

            var pedidoParaDTO = await _context.Pedidos
            .Include(p => p.DetallesPedido)
            .ThenInclude(d => d.Articulo)
            .Include(p => p.Domicilio)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

            PedidoTotalModificar(ref pedidoParaDTO);

            String mensaje ="";
            String grupoDestino="";
            PedidoDTO pedidoDTO=new();
            pedidoDTO.Cliente = pedidoParaDTO.Cliente;
            pedidoDTO.Domicilio = pedidoParaDTO.Domicilio;
            pedidoDTO.Estado = pedidoParaDTO.Estado;
            pedidoDTO.Fecha = pedidoParaDTO.Fecha;
            pedidoDTO.HoraEstimadaFin = pedidoParaDTO.HoraEstimadaFin;
            pedidoDTO.Id = pedidoParaDTO.Id;
            pedidoDTO.TipoEnvio = pedidoParaDTO.TipoEnvio;
            pedidoDTO.Total= pedidoParaDTO.Total;


            var cambio = (a: estadoPrevio, b: estadoActual);
                        
            switch (cambio)
            {
                // This case would be handle by FinalizarPedido
                //case (a: PENDIENTE, b: PENDIENTE):
                //    mensaje = "Su pedido esta pendiente de aprobacion";
                //    grupoDestino =  pedido.ClienteID.ToString();
                //    EnviarNotificacionCliente(grupoDestino, mensaje, pedidoDTO);
                //    break;

                case (a: PAGO_PENDIENTE_MP, b: PENDIENTE):
                    mensaje = "Su pedido esta pendiente de aprobacion";
                    grupoDestino = pedido.ClienteID.ToString();
                    EnviarNotificacionCliente(grupoDestino, mensaje, pedidoDTO);

                    mensaje = "Ha ingresado un nuevo pedido";
                    grupoDestino = _context.Roles.Where(r => r.Nombre == "Cajero").FirstOrDefault().Id.ToString();
                    EnviarNotificacionRol(grupoDestino, mensaje, pedidoDTO);
                    break;

                case (a: PENDIENTE, b: APROBADO):
                    mensaje = "Su pedido esta Aprobado y Facturado";
                    grupoDestino = pedido.ClienteID.ToString();
                    EnviarNotificacionCliente(grupoDestino, mensaje, pedidoDTO);

                    mensaje = "Ingresó pedido a cocinar";
                    grupoDestino = _context.Roles.Where(r=>r.Nombre=="Cocinero").FirstOrDefault().Id.ToString() ;
                    EnviarNotificacionRol(grupoDestino, mensaje, pedidoDTO);
                    
                    //si se aprueba el pedido, facturar el pedido
                   await Facturar(pedidoParaDTO.Id);

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
        public async Task<ActionResult<Pedido>> PostPedido(Pedido pedido)
        {
                 
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPedido", new { id = pedido.Id }, pedido);
        }


        // POST: api/Pedidos/Finalizar/5
        [HttpPost("Finalizar/{id}")]
        public async Task<ActionResult<PedidoDTO>> FinalizarPedido(long id)
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


            //Si el pedido es a domicili0, sumar los 10min extras 
            if (pedido.TipoEnvio == 1)
            {
                tiempoEntregaDelivery = 10;
            }

            Configuracion configuracion = await _context.Configuraciones.FirstOrDefaultAsync();
            int CocinerosCant = configuracion.CantidadCocineros;

            long FormulaTE= TECocinaPedidoActual.min + TECocinaPedidosConEstado.min / CocinerosCant  + tiempoEntregaDelivery ;


            //Calcular total del pedido 
            PedidoTotalModificar(ref pedido);

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

            //Si NO paga por mercadolibre, su estado es PENDIENTE
            if (pedidoDTO.Estado != PAGO_PENDIENTE_MP)
            {
                mensaje = "Ha ingresado un nuevo pedido";
                grupoDestino = _context.Roles.Where(r => r.Nombre == "Cajero").FirstOrDefault().Id.ToString();
                EnviarNotificacionRol(grupoDestino, mensaje, pedidoDTO);

                mensaje = "Su pedido esta pendiente de aprobacion";
                grupoDestino = pedido.ClienteID.ToString();
                EnviarNotificacionCliente(grupoDestino, mensaje, pedidoDTO);


            }
            

            //-------fin notificacion

            return (pedidoDTO);
        }

        private async Task Facturar(long id)
        {

            var pedidoNuevo = await _context.Pedidos
            .Include(p => p.DetallesPedido)
            .ThenInclude(d => d.Articulo)
            .Include(p => p.Domicilio)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

            //bool existeFacturaDelPedido = facturasController.GetFacturaDePedidoExiste(pedido.Id).Result.Value;

            //if (!existeFacturaDelPedido)
            //{
            Factura factura = new();
                factura.Fecha = DateTime.Now;
                factura.MontoDescuento = Convert.ToSingle(PedidoMontoDescuentoCalcular(pedidoNuevo));
                factura.PedidoID = pedidoNuevo.Id;
                factura.Total = Convert.ToDecimal(PedidoTotalCalcular(pedidoNuevo));
                factura.DetallesFactura = new List<DetalleFactura>();

                //Crear detalles de factura por cada detalle del pedido
                foreach (var DP in pedidoNuevo.DetallesPedido)
                {
                    DetalleFactura detalleFactura = new();
                    detalleFactura.DetallePedidoID = DP.Id;
                    detalleFactura.Factura = factura;
                    factura.DetallesFactura.Add(detalleFactura);
                }

            _context.Facturas.Add(factura);
            await _context.SaveChangesAsync();

            foreach (var detalleFactura in factura.DetallesFactura)
            {

            var detalleFacturaNuevo = await _context.DetallesFacturas
                .Include(c => c.DetallePedido)
                .ThenInclude(a => a.Articulo)
                .ThenInclude(a => a.Recetas)
                .ThenInclude(a => a.DetallesRecetas)
                .ThenInclude(a => a.Articulo)
                .Where(c => c.Id == detalleFactura.Id)
                .FirstOrDefaultAsync();

            for (int i = 1; i <= detalleFacturaNuevo.DetallePedido.Cantidad; i++)
            {
                if (detalleFacturaNuevo.DetallePedido.Articulo.EsManufacturado)
                {
                    foreach (var DR in detalleFacturaNuevo.DetallePedido.Articulo.Recetas.FirstOrDefault().DetallesRecetas)
                    {
                            if (DR.Disabled==false)
                            {
                            //await Egresar(DR.Articulo, DR.Cantidad, detalleFacturaNuevo.Id);
                            //Toma una ETERNIDAD para pedidos grandes
                            }
                    }
                }
            }

            }

            Factura facturaNueva = await _context.Facturas
                .Include(a => a.Pedido)
                .ThenInclude(a => a.Cliente)
                .ThenInclude(a => a.Usuario)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == factura.Id);

            string facturaHtml = await FacturaToHTML(factura.Id);
            string facturaPDF = HTML2PDF(facturaHtml);
            SendMail(facturaNueva.Pedido.Cliente.Usuario.NombreUsuario, facturaPDF);

            //} //if (!existeFacturaDelPedido)

        }

        private void SendMail(String correo, String attachmentFilePath )
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("ElBuenSaborUTN2021@gmail.com");
                mail.To.Add(correo);
                mail.Subject = "LaBuenaFactura ";
                mail.Body = "Factura";

                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(attachmentFilePath);
                mail.Attachments.Add(attachment);

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("ElBuenSaborUTN2021", "lddqvjtayidwimls");
                SmtpServer.EnableSsl = true;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.Send(mail);
                Console.WriteLine("Correo Enviado");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        private async Task<String> FacturaToHTML(long id)
        {

            Factura factura = await _context.Facturas
                .Include(a => a.Pedido)
                    .ThenInclude(a => a.DetallesPedido)
                    .ThenInclude(a => a.Articulo)
                .Include(a => a.Pedido.Domicilio)
                .Include(a => a.Pedido.Cliente)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);

            String DetallesFactura = "";
           
            foreach (var item in factura.Pedido.DetallesPedido)
            {
                var templateDetalle = new HtmlTemplate(@"DetalleFactura.html");
                var outputDetalle = templateDetalle.Render(new
                {
                    CODIGO = Convert.ToString(item.ArticuloID),
                    PRODUCTO = item.Articulo.Denominacion,
                    CANTIDAD = Convert.ToString(item.Cantidad),
                    UNIDADDEMEDIDA = item.Articulo.UnidadMedida,
                    PRECIOUNITARIO = Convert.ToString( Math.Floor( (item.Subtotal*10)/ item.Cantidad)/10),
                    SUBTOTAL = Convert.ToString(item.Subtotal),
                });

                    DetallesFactura += outputDetalle;
            }

            var template = new HtmlTemplate(@"FacturaTemplate.html");
            var output = template.Render(new
            {
                NUMERODEFACTURA =Convert.ToString( factura.Numero),
                FECHADEEMISION = Convert.ToString(factura.Fecha),
                APELLIDOYNOMBRE = factura.Pedido.Cliente.Apellido + ", " + factura.Pedido.Cliente.Nombre,
                CONDICIONDEVENTA = factura.Pedido.FormaPago ,
                DOMICILIO = factura.Pedido.Domicilio.Calle + " " + factura.Pedido.Domicilio.Numero + ", " + factura.Pedido.Domicilio.Localidad,
                IMPORTETOTAL = Convert.ToString(factura.Total),
                DETALLESDEFACTURA = DetallesFactura
            }); ;

            String newHtmlPath = @"F-" + Convert.ToString(factura.Numero) + " - " + factura.Pedido.Cliente.Apellido + " " + factura.Pedido.Cliente.Nombre + ".html";

            using (StreamWriter writetext = new StreamWriter(newHtmlPath))
            {
                writetext.WriteLine(output);
            }

            return newHtmlPath;
        }

        private String HTML2PDF(String facturaHtmlFileName)
        {

            // This will get the current WORKING directory (i.e. \bin\Debug)
            string workingDirectory = Environment.CurrentDirectory;
            // or: Directory.GetCurrentDirectory() gives the same result
            // This will get the current PROJECT bin directory (ie ../bin/)
            //string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;
            // This will get the current PROJECT directory
            //string projectDirectory2 = Directory.GetParent(workingDirectory).Parent.Parent.FullName;

            // read parameters from the webpage
            string url = workingDirectory + "\\" + facturaHtmlFileName;
            string pdfFile = workingDirectory + "\\wwwroot\\PDF\\" + facturaHtmlFileName.Split(".")[0] + ".pdf";

            // instantiate the html to pdf converter
            HtmlToPdf converter = new HtmlToPdf();

            //A standard A4 page has 595 x 842 points. 1 point is 1/72 inch. 1 pixel is 1/96 inch. This means that an A4 page width is 793px. 
            converter.Options.WebPageWidth = 793;
            converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;

            // set converter rendering engine
            converter.Options.RenderingEngine = RenderingEngine.Blink;

            // set document passwords
            converter.Options.SecurityOptions.OwnerPassword = "pass1";
            //converter.Options.SecurityOptions.UserPassword = "pass2";

            //set document permissions
            converter.Options.SecurityOptions.CanAssembleDocument = false;
            converter.Options.SecurityOptions.CanCopyContent = true;
            converter.Options.SecurityOptions.CanEditAnnotations = true;
            converter.Options.SecurityOptions.CanEditContent = false;
            converter.Options.SecurityOptions.CanFillFormFields = false;
            converter.Options.SecurityOptions.CanPrint = true;

            // convert the url to pdf
            PdfDocument doc = converter.ConvertUrl(url);
           
            // save pdf document
            doc.Save(pdfFile);

            // close pdf document
            doc.Close();

            return pdfFile;

        }

        private async Task Egresar(Articulo articulo, double cantidad, long DFid)
        {
            Console.WriteLine("  ");
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("Articulo: " + articulo.Denominacion + " - Cantidad: " + cantidad + " - DFid: " + DFid);
            Console.WriteLine("  ");
            Console.WriteLine("Stock ante de egreso:");
            int cantidadQueFaltaEgresar = (int) cantidad;
            var stock = await _context.Stocks
                .Where(a => a.ArticuloID == articulo.Id)
                .Where(b => b.CantidadDisponible > 0)
                .Where(c => c.Disabled == false)
                .OrderBy(d => d.FechaCompra).ToListAsync();

            foreach (var item in stock)
            {
                Console.WriteLine("Fecha: " + item.FechaCompra +  " - Articulo: "+item.Articulo.Denominacion + " - Cantidad: " + item.CantidadDisponible);
            }
            
            int i = 0;
            while (cantidadQueFaltaEgresar>0)
            {

                EgresoArticulo egresoArticulo = new();
                
                if (cantidadQueFaltaEgresar >= stock[i].CantidadDisponible)
                {
                    cantidadQueFaltaEgresar -= stock[i].CantidadDisponible;
                    egresoArticulo.CantidadEgresada = stock[i].CantidadDisponible;
                    stock[i].CantidadDisponible = 0;
                }
                else if (cantidadQueFaltaEgresar < stock[i].CantidadDisponible)
                {
                    stock[i].CantidadDisponible -= cantidadQueFaltaEgresar;
                    egresoArticulo.CantidadEgresada = cantidadQueFaltaEgresar;
                    cantidadQueFaltaEgresar = 0;
                }
                //Guardar el Egreso
                egresoArticulo.StockID = stock[i].Id;
                egresoArticulo.DetalleFacturaID = DFid;
                _context.EgresosArticulos.Add(egresoArticulo);
                await _context.SaveChangesAsync();

                //Guardar modificaciones la tabla de Stock
                _context.Entry(stock[i]).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                i++;
            }

            Console.WriteLine("  ");
            Console.WriteLine("Stock luego de egreso:");
            foreach (var item in stock)
            {
                Console.WriteLine("Fecha: " + item.FechaCompra + " - Articulo: " + item.Articulo.Denominacion + " - Cantidad: " + item.CantidadDisponible);
            }


        }


        // POST: api/Pedidos/MercadoPagoPreference
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("MercadoPagoPreference")]
        public async Task<ActionResult<Preference>> MercadoPago([FromBody] dynamic preferencia)
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
                        UnitPrice = preferencia.total,
                    },
                },
                BackUrls = new PreferenceBackUrlsRequest
                {
                    Success = "http://localhost:8080/cliente/Carrito/aprobado",
                    Failure = "http://localhost:8080/cliente/Carrito/aprobado",
                    Pending = "http://localhost:8080/cliente/Carrito/aprobado"
                },
                ExternalReference = Convert.ToString(preferencia.pedidoId),
                //no se puede configurar que haga notificaciones a localHost, debe ser una url publica
                // NotificationUrl = "https://localhost:44350/api/Pedidos/MercadoPagoNotificacion",
            };

            // Crea la preferencia usando el client
            var client = new PreferenceClient();
            Preference preference = await client.CreateAsync(request);
            
            return preference;
        }


        // POST: api/Pedidos/MercadoPagoNotificacion?topic=payment&id=123456789
        [HttpPost("MercadoPagoNotificacion")]
        public async Task<ActionResult<String>> MercadoPagoNotificacion([FromQuery] string topic, [FromQuery] long id)
        {
            using var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization =
             new AuthenticationHeaderValue("Bearer", "TEST-5059945658019779-070913-a1924cb562898b6ed9191db0f41badf6-155784029");

            var result = await httpClient.GetAsync("https://api.mercadopago.com/v1/payments/"+id);
            Console.WriteLine(result.StatusCode);

            return StatusCode(200);
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

        private double PedidoMontoDescuentoCalcular(Pedido pedido)
        {
            double SumaDeDetallesPedido = 0;

            //Calcular total del pedido 
            foreach (var detalle in pedido.DetallesPedido)
            {
                SumaDeDetallesPedido += Convert.ToDouble(detalle.Subtotal);
            }

            if (pedido.TipoEnvio == LOCAL)
            {
                return SumaDeDetallesPedido * 0.1;
            }

            return 0;
        }

        private double PedidoTotalCalcular(Pedido pedido)
        {
            double Total=0;
            double SumaDeDetallesPedido = 0;

            //no deberia ocurrir
            if (pedido.DetallesPedido==null)
            {
                return 0;
            }

            //Calcular total del pedido 
            foreach (var detalle in pedido.DetallesPedido)
            {
                SumaDeDetallesPedido += Convert.ToDouble(detalle.Subtotal);
            }

            Total = SumaDeDetallesPedido;

            if (pedido.TipoEnvio==LOCAL)
            {
                Total = SumaDeDetallesPedido * 0.9;
            }

            return Total;

        }

        private void PedidoTotalModificar(ref Pedido pedido)
        {
            pedido.Total=PedidoTotalCalcular(pedido);
        }

    }
}
