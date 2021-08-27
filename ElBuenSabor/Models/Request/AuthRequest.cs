using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSabor.Models.Request
{
    public class AuthRequest
    {
        [Required] //Indica que este campo es requerido
        public String NombreUsuario { get; set; }
        [Required]
        public String Clave { get; set; }
    }
}
