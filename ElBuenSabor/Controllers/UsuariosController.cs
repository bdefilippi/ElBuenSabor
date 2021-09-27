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
using ElBuenSabor.Tools;
using Microsoft.AspNetCore.Authorization;

namespace ElBuenSabor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Cliente,Administrador,Cajero,Cocinero")]

    public class UsuariosController : ControllerBase
    {
        private readonly ElBuenSaborContext _context;

        /*es buena practica anteponer un guion bajo para variables privadas
         para asi saber cuando lee en codigo abajo que se trata de una variable privada.
        La interfaz sirve para que se mantenga la forma de la funcion pero pueda cambiarse
        en algun momento la logica de autenticacion. Solo cambiando la clase en Startup que
        implementa la interface y la inyecta es suficiente para cambiar el tipo de autenticacion
         */
        private readonly IAuthService _authService;
        
        public UsuariosController(ElBuenSaborContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(long id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // PUT: api/Usuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> PutUsuario(UsuarioChange usuarioChange)
        {

            //Revisa si el usuario figura en los registros de la base de datos
            string hashPassword = Encrypt.GetSHA256(usuarioChange.ClaveVieja);
            bool existe = _context.Usuarios.Any(u => u.NombreUsuario == usuarioChange.NombreUsuarioViejo && u.Clave == hashPassword);

            if (!existe)
            {
                return BadRequest();
            }

            Usuario usuario = _context.Usuarios.Where(x => x.NombreUsuario == usuarioChange.NombreUsuarioViejo).FirstOrDefault();

            usuario.NombreUsuario = usuarioChange.NombreUsuarioNuevo;
            usuario.Clave = Encrypt.GetSHA256(usuarioChange.ClaveNueva);

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                    return NotFound();
                            }

            return NoContent();
        }

        // POST: api/Usuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuario", new { id = usuario.Id }, usuario);
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteUsuario(long id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(long id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }




    }
}
