using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSabor.Models
{
    public class DetallePedido
    {
        public long Id { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
        public bool Disabled { get; set; }
        public long PedidoID { get; set; }
        public Pedido Pedido { get; set; }
        public long ArticuloID { get; set; }
        public Articulo Articulo { get; set; }
        public int Estado { get; set; }

    }
}
