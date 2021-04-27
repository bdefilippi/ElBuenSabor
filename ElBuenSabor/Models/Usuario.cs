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
        public String Rol { get; set; }
        public bool IsEnabled { get; set; }
    }
}
