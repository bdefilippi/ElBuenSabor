using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSabor.Models
{
    public class Configuracion
    {

        public long Id { get; set; }
        public int CantidadCocineros { get; set; }
        public String EmailEmpresa { get; set; }
        public String TokenMercadoPago { get; set; }
        public bool Disabled { get; set; }


    }
}
