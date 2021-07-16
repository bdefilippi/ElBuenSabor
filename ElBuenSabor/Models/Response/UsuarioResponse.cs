using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSabor.Models.Response
{
    public class UsuarioResponse
    {
        public string NombreUsuario { get; set; }
        public string Rol { get; set; }
        public string Token { get;  set; }
        public object Cliente { get; set; }

        /*El papeleo para identificarte es el login
        Una vez hecho el papeleo para identificarte 
        te dan una credencial. Esa credencial se llama
        Token. Luego cada vez que quieras pasar, muestras la 
        credencial
         */
    }
}
