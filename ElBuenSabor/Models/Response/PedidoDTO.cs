using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSabor.Models.Response
{
    public class PedidoDTO
    {
        public long Id { get; set; }
        public DateTime Fecha { get; set; }
        public int Estado { get; set; }
        public DateTime HoraEstimadaFin { get; set; }
        public int TipoEnvio { get; set; }
        public double Total { get; set; }
        public Cliente Cliente { get; set; }
        public Domicilio Domicilio { get; set; }
   

    }
}
