using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSabor.Models
{
    public class DetalleFactura
    {
        public long Id { get; set; }
        [NotMapped]
        public double Subtotal { get; set; }
        public bool Disabled { get; set; }
        public long FacturaID { get; set; }
        public Factura Factura { get; set; }    //composicion
        public long DetallePedidoID { get; set; }
        public DetallePedido DetallePedido { get; set; }
    }
}
