using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSabor.Models
{
    public class DetallePedido
    {
        public long Id { get; set; }
        public int Cantidad { get; set; }
        public double Subtotal { get; set; }
        public bool IsEnabled { get; set; }
        public long IdPedido { get; set; }
        public Pedido Pedido { get; set; }
        public long IdArticuloManufacturado { get; set; }
        public ArticuloManufacturado ArticuloManufacturado { get; set; }
        public long IdArticuloInsumo { get; set; }
        public ArticuloInsumo ArticuloInsumo { get; set; }

    }
}
