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
        public long PedidoID { get; set; }
        public Pedido Pedido { get; set; }
        public long ArticuloManufacturadoID { get; set; }
        public ArticuloManufacturado ArticuloManufacturado { get; set; }
        public long ArticuloInsumoID { get; set; }
        public ArticuloInsumo ArticuloInsumo { get; set; }

    }
}
