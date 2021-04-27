using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSabor.Models
{
    public class DetalleFactura
    {
        public long Id { get; set; }
        public int Cantidad { get; set; }
        public double Subtotal { get; set; }
        public bool IsEnabled { get; set; }
        public long IdFactura { get; set; }
        public Factura Factura { get; set; }    //composicion
        public long IdArticuloManufacturado { get; set; }
        public ArticuloManufacturado ArticuloManufacturado { get; set; }
        public long IdArticuloInsumo { get; set; }
        public ArticuloInsumo ArticuloInsumo { get; set; }
    }
}
