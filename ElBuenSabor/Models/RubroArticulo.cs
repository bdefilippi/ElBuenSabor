using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSabor.Models
{
    public class RubroArticulo
    {
        public long Id { get; set; }
        public String Denominacion { get; set; }
        public bool IsEnabled { get; set; }
        public ICollection<ArticuloInsumo> ArticulosInsumo { get; set; }
        public long RubroPadreID { get; set; }
        public RubroArticulo RubroPadre { get; set; }
        public ICollection<RubroArticulo> RubrosHijos { get; set; }

    }
}
