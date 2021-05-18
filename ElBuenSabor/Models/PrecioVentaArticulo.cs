using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSabor.Models
{
    public class PrecioVentaArticulo
    {
        public long Id { get; set; }
        public int PrecioVenta { get; set; }
        public DateTime Fecha { get; set; }
        public long ArticuloID { get; set; }
        public Articulo Articulo { get; set; }


    }
}
