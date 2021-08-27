using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSabor.Models
{
    public class EgresoArticulo
    {
        public long Id { get; set; }
        public int CantidadEgresada { get; set; }
        public long StockID { get; set; }
        public Stock Stock { get; set; }
        public long DetallePedidoId { get; set; }
        public DetallePedido DetallePedido { get; set; }
        public bool Disabled { get; set; }

    }
}
