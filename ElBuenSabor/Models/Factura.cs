using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSabor.Models
{
    public class Factura
    {
        public long Id { get; set; }
        public long Numero { get; set; }
        public DateTime Fecha { get; set; }
        public Double MontoDescuento { get; set; }
        public bool Disabled { get; set; }
        public long PedidoID { get; set; }
        public Pedido Pedido { get; set; }
        [NotMapped]
        public double TotalCosto { get; set; }
        public decimal Total { get; set; }
        public ICollection<DetalleFactura> DetallesFactura { get; set; }    //Es composicion
    }
}
