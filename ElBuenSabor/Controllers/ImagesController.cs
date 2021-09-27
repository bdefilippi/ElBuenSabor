using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElBuenSabor.Models;

//-------p/Imagen
using Microsoft.AspNetCore.Hosting;
using System.IO;
//-------p/Imagen

//-------Jwt
using Microsoft.AspNetCore.Authorization;
//-------Jwt

namespace ElBuenSabor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ImagesController : ControllerBase
    {
        private readonly ElBuenSaborContext _context;
        private static IWebHostEnvironment _environment; //Permite acceder a la carpeta del servidor para guardar imagenes

        public ImagesController(ElBuenSaborContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: /api/Images/default.png
        [HttpGet("{fileName}")]
        public IActionResult GetImage(string fileName)
        {

            string path = Path.Combine(_environment.ContentRootPath, "../ebsa/wwwroot/images/" + fileName);
            string defaultPath = Path.Combine(_environment.ContentRootPath, "../ebsa/wwwroot/images/" + "default.png");
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

        // GET: /api/Images/
        [HttpGet]
        public IActionResult GetDefault()
        {
            return GetImage("");
        }

    }
}
