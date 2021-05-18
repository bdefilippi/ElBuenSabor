using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSabor.Models
{
    public class Rol
    {
        public long Id { get; set; }
        public string Nombre { get; set; }
        public bool Disabled { get; set; }
        public ICollection<Usuario> Usuarios { get; set; }

    }
}
