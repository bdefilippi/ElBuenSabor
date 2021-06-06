using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElBuenSabor.Models;

using System.Data;
using Microsoft.Data;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace ElBuenSabor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticulosController : ControllerBase
    {



        private readonly ElBuenSaborContext _context;

        public ArticulosController(ElBuenSaborContext context)
        {
            _context = context;
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
            //var articulo = await _context.Articulos.FindAsync(id);

            var articulo = await _context.Articulos
                .Include(s => s.Stocks)
                .Include(r => r.Recetas)
                .Include(d => d.RubroArticulo)
                .Include(p => p.PreciosVentaArticulos) //probar order by descending
                .FirstOrDefaultAsync(c => c.Id == id);

            var stocks = articulo.Stocks;

            articulo.StockActual = CalcularStockActual(stocks);

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

        // POST: /api/Articulos/CostoReceta/1
        [HttpGet("CostoReceta/{id}")]
        public String PostArticuloCosto(long id)
        {
            string connectionString = "Server=den1.mssql8.gear.host;Database=elbuensabordb; Trusted_Connection=False;User Id=elbuensabordb; Password=#Base007;";
            String Resupuesta = "";
            string queryString = "EXECUTE BuscarCostosIngredientesDeArticuloPorId @pricePoint";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@pricePoint", id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                Resupuesta=sqlDatoToJson(reader);
                //while (reader.Read())
                //{
                //    //Console.WriteLine("\t{0}", reader[0]);
                //    Resupuesta = Convert.ToString(reader[0]);
                //}
                reader.Close();
            }
            return Resupuesta;
        }

        private String sqlDatoToJson(SqlDataReader dataReader)
        {
            var dataTable = new DataTable();
            dataTable.Load(dataReader);
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(dataTable);
            return JSONString;
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

        [NonAction]
        private int CalcularStockActual(ICollection<Stock> stocks)
        {
            var sumatoria = 0;

            foreach (var stock in stocks)
            {
                sumatoria += stock.CantidadDisponible;
            }

            return sumatoria;
        }
    }
}
