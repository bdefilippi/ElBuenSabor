using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElBuenSabor.Models;

using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace ElBuenSabor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticulosController : ControllerBase
    {
        private readonly ElBuenSaborContext _context;

        private static IWebHostEnvironment _environment; //Permite acceder a la carpeta del servidor para guardar imagenes

        public ArticulosController(ElBuenSaborContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }



        // GET: api/Articulos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Articulo>>> GetArticulos()
        {
            return await _context.Articulos.ToListAsync();
        }

        // GET: api/Articulos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Articulo>> GetArticulo(long id)
        {
            var articulo = await _context.Articulos.FindAsync(id);

            if (articulo == null)
            {
                return NotFound();
            }

            return articulo;
        }

        // PUT: api/Articulos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticulo(long id, Articulo articulo)
        {
            if (id != articulo.Id)
            {
                return BadRequest();
            }

            _context.Entry(articulo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticuloExists(id))
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

        // POST: api/Articulos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Articulo>> PostArticulo(Articulo articulo)
        {
            _context.Articulos.Add(articulo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArticulo", new { id = articulo.Id }, articulo);
        }

        // DELETE: api/Articulos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticulo(long id)
        {
            var articulo = await _context.Articulos.FindAsync(id);
            if (articulo == null)
            {
                return NotFound();
            }

            _context.Articulos.Remove(articulo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArticuloExists(long id)
        {
            return _context.Articulos.Any(e => e.Id == id);
        }


        //---------------------imagen-------------------------

        // POST: /api/Articulos/UploadImage/1
        [HttpPost("UploadImage/{id}"), DisableRequestSizeLimit]
        public async Task<string> UploadFile([FromForm] IFormFile image, long id)
        {
            
            string path = Path.Combine(_environment.ContentRootPath, "Images/" + image.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            AsignarImagen(image.FileName, id).Wait();
            return image.FileName;

        }

        private async Task<bool> AsignarImagen(string FileName, long id)
        {
            var articulo =  GetArticulo(id).Result.Value;
            articulo.Imagen = FileName;
            await PutArticulo(id, articulo);
            return true;
        }

        // GET: /api/Articulos/Image/default.png
        [HttpGet("Image/{fileName}")]
        public IActionResult GetImage(string fileName)
        {

            string path = Path.Combine(_environment.ContentRootPath, "Images/" + fileName);
            string defaultPath = Path.Combine(_environment.ContentRootPath, "Images/" + "default.png");
            Console.WriteLine(fileName);
            try
            {
                var image = System.IO.File.OpenRead(path);
                return File(image, "image/jpeg");
            }
            catch (Exception)
            {
            }

            var imageDefault = System.IO.File.OpenRead(defaultPath);
            return File(imageDefault, "image/jpeg");
        }

        // GET: /api/Articulos/Image/
        [HttpGet("Image/")]
        public IActionResult GetDefault()
        {
            return GetImage("");
        }

        //-------------------fin imagen-----------------------


    }
}
