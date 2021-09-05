using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSabor.Models.Response
{

    /*El papeleo para identificarte es el login. Una vez hecho el papeleo para identificarte 
    te dan una credencial. Esa credencial se llama Token. Luego cada vez que quieras pasar, 
    muestras la credencial/token.
     */

    public class AuthResponse
    {
        public UsuarioDTO Usuario { get; set; }
        public ClienteDTO Cliente { get; set; }
        public string Token { get; set; }

        public AuthResponse()
        {
            Usuario = new();
            Cliente = new();
        }

    }



    public class ClienteDTO
    {
        public long id { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public long telefono { get; set; }
        public ICollection<Domicilio> domicilios { get; set; }
    }

    public class UsuarioDTO
    {
        public string NombreUsuario { get; set; }
        public long RolID { get; set; }
        public string Rol { get; set; }
    }
}
