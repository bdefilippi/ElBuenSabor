using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSabor.Models
{
    public class RubroGeneral
    {
        public long Id { get; set; }
        public String Denominacion { get; set; }
        public bool IsEnabled { get; set; }
        public ICollection<ArticuloManufacturado> ArticulosManufacturados { get; set; }

    }
}
