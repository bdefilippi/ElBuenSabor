using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElBuenSabor.Models;
using ElBuenSabor.Models.Request;
using ElBuenSabor.Services;
using ElBuenSabor.Models.Response;
using Google.Apis.Auth;
using ElBuenSabor.Models.Common;
using ElBuenSabor.Tools;

namespace ElBuenSabor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ElBuenSaborContext _context;
        private readonly IAuthService _authService;
        private readonly GoogleAuthSettings _GoogleAuthSettings;
        private readonly CommonPassSettings _CommonPassSettings;

        public AuthController(ElBuenSaborContext context, IAuthService authService, GoogleAuthSettings GoogleAuthSettings, CommonPassSettings CommonPassSettings)
        {
            _context = context;
            _authService = authService;
            _GoogleAuthSettings = GoogleAuthSettings;
            _CommonPassSettings = CommonPassSettings;
        }
        
        //-------------------- autenticacion--------------
        [HttpPost("login")]
        public IActionResult Autentificar([FromBody] AuthRequest authRequest)
        {

            //quiero que genere un authResponse cuando se autentifique
            var authResponse = _authService.Authorize(authRequest);

            if (authResponse == null)
            {
                return BadRequest("Usuario o contraseña incorrecta");
            }
            return Ok(authResponse);

        }
        //----------------------fin autenticacion--------------------------





        //--------------------Google autenticacion--------------
        [HttpPost("googlelogin")]
        public IActionResult GoogleAutentificar([FromBody] GoogleAuthRequest googleAuthRequest)
        {
            AuthRequest authRequest = new();
            AuthResponse authResponse = new();

            try
            {
                //The authentication API will use the idToken from google and verify it.
                GoogleJsonWebSignature.ValidationSettings settings = new GoogleJsonWebSignature.ValidationSettings();

                //Here goes the app google client ID
                settings.Audience = new List<string>() { "225689514544-qccdbtr164tekpjkgq0fn1f7630g2266.apps.googleusercontent.com" };

                GoogleJsonWebSignature.Payload payload = GoogleJsonWebSignature.ValidateAsync(googleAuthRequest.Id_token, settings).Result;


                //***LOGIN***
                //Revisa si el usuario figura en los registros de la base de datos, si es asi lo logea
                string hashCommonPassword = Encrypt.GetSHA256(_CommonPassSettings.Pass);
                bool existe = _context.Usuarios.Any(u => u.NombreUsuario == payload.Email && u.Clave == hashCommonPassword);

                if (existe)
                {
                    //Then it creates an access token that grants access to the other APIs of your app.
                    authRequest.NombreUsuario = payload.Email;
                    authRequest.Clave = _CommonPassSettings.Pass;

                    authResponse = _authService.Authorize(authRequest);

                    return Ok( _authService.Authorize(authRequest) );
                }


                //***REGISTRER...***
                //Agregamos el usuario nuevo
                Usuario usuarioNuevo = new();
                usuarioNuevo.NombreUsuario = payload.Email;
                usuarioNuevo.Clave = hashCommonPassword;
                usuarioNuevo.RolId = 1; //Corresponde al Cliente en la base de datos de Roles
                _context.Usuarios.Add(usuarioNuevo);
                _context.SaveChanges(); //Debería hacerse de forma asincrona, cambiar eso

                //Luego agregamos el cliente nuevo
                Cliente clienteNuevo = new();
                clienteNuevo.Nombre = payload.GivenName;
                clienteNuevo.Apellido = payload.FamilyName;
                clienteNuevo.Telefono = 0;
                clienteNuevo.UsuarioID = usuarioNuevo.Id;
                _context.Clientes.Add(clienteNuevo);
                _context.SaveChanges(); //Debería hacerse de forma asincrona, cambiar eso

                //***...then LOGIN***
                //Then it creates an access token that grants access to the other APIs of your app.
                authRequest.NombreUsuario = payload.Email;
                authRequest.Clave = hashCommonPassword;

                authResponse = _authService.Authorize(authRequest);

                return Ok(_authService.Authorize(authRequest) );

            }
            catch
            {
                Console.WriteLine("Error en GoogleAuth");
            }
            return StatusCode(500);

        }
        //----------------------fin autenticacion--------------------------

    }
}
