using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSabor.Models
{
    public class DetalleReceta
    {
        public long Id { get; set; }
        public double Cantidad { get; set; }
        public long ArticuloID { get; set; }
        public Articulo Articulo { get; set; }
        public long RecetaID { get; set; }
        public Receta Receta { get; set; }


    }
}
