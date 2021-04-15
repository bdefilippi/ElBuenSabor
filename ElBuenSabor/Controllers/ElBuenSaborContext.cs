using ElBuenSabor.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSabor.Controllers
{
    public class ElBuenSaborContext : DbContext
    {
        public ElBuenSaborContext(DbContextOptions<ElBuenSaborContext> options)
            :base(options)
        {

        }

        public DbSet<ArticuloManufacturado> ArticulosManufacturados { get; set; }
    }
}
