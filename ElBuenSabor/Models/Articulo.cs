using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSabor.Models
{
    public class Articulo
    {
        public long Id { get; set; }
        public String Denominacion { get; set; }
        public String Imagen { get; set; }
        public String UnidadMedida { get; set; }
        public double StockMinimo { get; set; }
        [NotMapped]
        public double StockActual { get; set; }
        public int TiempoEstimadoCocina { get; set; }
        public bool ALaVenta { get; set; }
        public bool Disabled { get; set; }
        public long RubroArticuloID { get; set; }
        public RubroArticulo RubroArticulo { get; set; }
        public ICollection<PrecioVentaArticulo> PreciosVentaArticulos { get; set; }
        public ICollection<Receta> Recetas { get; set; }
        public ICollection<DetalleReceta> DetallesRecetas { get; set; }
        public ICollection<Stock> Stocks { get; set; }
        public ICollection<DetallePedido> DetallesPedidos { get; set; }
        public ICollection<DetalleFactura> DetallesFacturas { get; set; }

    }
}
