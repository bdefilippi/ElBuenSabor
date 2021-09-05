using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSabor.Models
{
    public class Usuario
    {
        public long Id { get; set; }
        public String NombreUsuario { get; set; }
        public String Clave { get; set; }
        public bool Disabled { get; set; }
        public long RolID { get; set; }
        public Rol Rol { get; set; }


    }
}
