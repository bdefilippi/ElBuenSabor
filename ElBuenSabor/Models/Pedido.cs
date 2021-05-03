using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSabor.Models
{
    public class Pedido
    {
        public long Id { get; set; }
        public long Numero { get; set; }
        public DateTime Fecha { get; set; }
        public int Estado { get; set; }
        public DateTime HoraEstimadaFin { get; set; }
        public int TipoEnvio { get; set; }
        public double Total { get; set; }
        public long ClienteID { get; set; }
        public Cliente Cliente { get; set; }
        public long DomicilioID { get; set; }
        public Domicilio Domicilio { get; set; }
        public long MercadoPagoDatosID { get; set; }
        public MercadoPagoDatos MercadoPagoDatos { get; set; }
        public long FacturaID { get; set; }
        public Factura Factura { get; set; }
        public bool IsEnabled { get; set; }
        public ICollection<DetallePedido> DetallesPedido { get; set; }  //composicion


    }
}
