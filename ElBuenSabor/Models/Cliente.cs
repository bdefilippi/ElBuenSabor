using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSabor.Models
{
    public class Cliente
    {
        public long Id { get; set; }
        public String Nombre { get; set; }
        public String Apellido { get; set; }
        public long Telefono { get; set; }
        public String Email { get; set; }
        public long IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
        public long IdDomicilio { get; set; }
        public Domicilio Domicilio { get; set; }
        public ICollection<Pedido> Pedidos { get; set; }
        public bool IsEnabled { get; set; }

    }
}
