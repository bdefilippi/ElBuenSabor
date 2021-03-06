using ElBuenSabor.Controllers;
using ElBuenSabor.Models;
using ElBuenSabor.Models.Common;
using ElBuenSabor.Models.Request;
using ElBuenSabor.Models.Response;
using ElBuenSabor.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ElBuenSabor.Services
{
    public class AuthService : IAuthService
    {

        private readonly JwtSettings _JwtSettings;
        
        private readonly ElBuenSaborContext _context;

        public AuthService(JwtSettings JwtSettings, ElBuenSaborContext context)
        {
            _JwtSettings = JwtSettings;
            _context = context;
        }

        public async Task<AuthResponse> Authorize(AuthRequest authRequest)
        {
            AuthResponse authResponse = new();
            {
                string spassword =  Encrypt.GetSHA256(authRequest.Clave);

                //Si el Admin creo una cuenta con password = "0" entonces con lo primero que se loggue será la contraseña
                var usuarioSinPassword = await _context.Usuarios
                    .Include(r => r.Rol)
                    .Where(u => u.NombreUsuario == authRequest.NombreUsuario && u.Clave == "0")
                    .FirstOrDefaultAsync();
                
                if (usuarioSinPassword != null)
                {
                    usuarioSinPassword.Clave = spassword;
                    _context.Entry(usuarioSinPassword).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }

                var usuario = await _context.Usuarios
                    .Include(r => r.Rol)
                    .Where(u => u.NombreUsuario == authRequest.NombreUsuario && u.Clave == spassword)
                    .FirstOrDefaultAsync();

                if (usuario == null) return null;

                var cliente = _context.Clientes
                    .Include(d => d.Domicilios)
                    .Where(u => u.UsuarioID == usuario.Id)
                    .Select(c => new {
                        id= c.Id,
                        nombre = c.Nombre,
                        apellido = c.Apellido,
                        telefono = c.Telefono,
                        domicilios = c.Domicilios
                    })
                    .FirstOrDefault();

                authResponse.Usuario.NombreUsuario = usuario.NombreUsuario;
                authResponse.Usuario.Rol = usuario.Rol.Nombre;
                authResponse.Usuario.RolID = usuario.RolID;

                authResponse.Token = CreateUserAuthToken(usuario);

                if (cliente != null)
                {
                    authResponse.Cliente.id = cliente.id;
                    authResponse.Cliente.nombre = cliente.nombre;
                    authResponse.Cliente.apellido = cliente.apellido;
                    authResponse.Cliente.telefono = cliente.telefono;
                    authResponse.Cliente.domicilios = cliente.domicilios;
                }
               

            }
            return  authResponse;
        }

        private string CreateUserAuthToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var llave = Encoding.ASCII.GetBytes(_JwtSettings.Secreto);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                        new Claim(ClaimTypes.Name,usuario.NombreUsuario.ToString()),
                        new Claim(ClaimTypes.Role,usuario.Rol.Nombre.ToString())
                    }
                    ),
                Expires=DateTime.UtcNow.AddDays(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(llave),SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
