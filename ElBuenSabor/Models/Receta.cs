using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSabor.Models
{
    public class Receta
    {
        public long Id { get; set; }
        public int TiempoEstimadoCocina { get; set; }
        public string Descripcion { get; set; }
        public bool Disabled { get; set; }
        public long ArticuloID { get; set; }
        public Articulo Articulo { get; set; }
        public ICollection<DetalleReceta> DetallesRecetas { get; set; }
    }
