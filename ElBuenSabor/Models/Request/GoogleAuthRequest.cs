using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSabor.Models.Request
{
    public class GoogleAuthRequest
    {
            [Required] //Indica que este campo es requerido
            public String Id_token { get; set; }
    }
}
