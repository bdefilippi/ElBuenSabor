using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSabor.Models.Request
{
    public class UsuarioChange
    {
        [Required] 
        public String NombreUsuarioViejo { get; set; }
        [Required]
        public String ClaveVieja { get; set; }
        [Required]
        public String NombreUsuarioNuevo { get; set; }
        [Required]
        public String ClaveNueva { get; set; }

    }
}
