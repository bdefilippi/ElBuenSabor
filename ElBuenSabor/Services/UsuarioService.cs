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
    public class UsuarioService : IUsuarioService
    {

        private readonly JwtSettings _JwtSettings;
        private readonly ElBuenSaborContext _context;

        public UsuarioService(JwtSettings JwtSettings, ElBuenSaborContext context)
        {
            _JwtSettings = JwtSettings;
            _context = context;
        }

        public UsuarioResponse Auth(AuthRequest model)
        {
            UsuarioResponse usuarioResponse = new UsuarioResponse();
            {
                string spassword = Encrypt.GetSHA256(model.Clave);
                var usuario = _context.Usuarios
                    .Include(r => r.Rol)
                    .Where(u => u.NombreUsuario == model.NombreUsuario && u.Clave == spassword)
                    .FirstOrDefault();
                if (usuario == null) return null;

                var cliente = _context.Clientes
                    .Include(d => d.Domicilios)
                    .Where(u => u.UsuarioID == usuario.Id)
                    .Select(c => new {
                        nombre = c.Nombre,
                        apellido = c.Apellido,
                        telefono = c.Telefono,
                        email = c.Email,
                        domicilios=c.Domicilios
                    })
                    .FirstOrDefault();

                usuarioResponse.NombreUsuario = usuario.NombreUsuario;
                usuarioResponse.Token = GetToken(usuario);
                usuarioResponse.Rol = usuario.Rol.Nombre;
                usuarioResponse.Cliente = cliente;
            }
            return usuarioResponse;
        }

        private string GetToken(Usuario usuario)
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
