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
        public bool Disabled { get; set; }
        public ICollection<Articulo> Articulos { get; set; }

    }
}
