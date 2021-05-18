using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSabor.Models
{
    public class Domicilio
    {
        public long Id { get; set; }
        public String Calle { get; set; }
        public int Numero { get; set; }
        public String Localidad { get; set; }
        public bool Disabled { get; set; }
        public ICollection<Pedido> Pedidos { get; set; }
        public long ClienteID { get; set; }
        public Cliente Cliente { get; set; }
    }
}
