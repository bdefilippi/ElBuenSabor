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
        public double MontoDescuento { get; set; }
        public String FormaPago { get; set; }
        [NotMapped]
        public double TotalVenta { get; set; }
        [NotMapped]
        public double TotalCosto { get; set; }
        public bool Disabled { get; set; }
        public ICollection<DetalleFactura> DetallesFactura { get; set; }    //Es composicion

    }
}
