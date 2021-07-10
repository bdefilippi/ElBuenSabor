﻿using System;
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

namespace ElBuenSabor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ElBuenSaborContext _context;

        /*es buena practica anteponer un guion bajo para variables privadas
         para asi saber cuando lee en codigo abajo que se trata de una variable privada.
        La interfaz sirve para que se mantenga la forma de la funcion pero pueda cambiarse
        en algun momento la logica de autenticacion. Solo cambiando la clase en Startup que
        implementa la interface y la inyecta es suficiente para cambiar el tipo de autenticacion
         */
        private readonly IUsuarioService _usuarioService;
        
        public UsuariosController(ElBuenSaborContext context, IUsuarioService usuarioService)
        {
            _context = context;
            _usuarioService = usuarioService;
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

        // PUT: api/Usuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(long id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Usuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuario", new { id = usuario.Id }, usuario);
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
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

        //-------------------- autenticacion--------------
        [HttpPost("login")]
        public IActionResult Autentificar([FromBody] AuthRequest model )
        {
            Respuesta respuesta = new Respuesta();

            //quiero que genere un usuarioResponse cuando se autentifique
            var usuarioResponse = _usuarioService.Auth(model);

            if (usuarioResponse == null)
            {
                respuesta.Exito = 0;
                respuesta.Mensaje = "Usuario o contraseña incorrecta";
                return BadRequest(respuesta);
            }
            respuesta.Exito = 1;
            respuesta.Data = usuarioResponse;

            return Ok(respuesta);
        }
        //------------------------------------------------

    }
}
