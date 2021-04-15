using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSabor.Models
{
    public class ArticuloManufacturado
    {
        public long Id { get; set; }
        public int TiempoEstimadoCocina { get; set; }
        public String Denominacion { get; set; }
        public double PrecioVenta { get; set; }
        public String Imagen { get; set; }

    }
}
