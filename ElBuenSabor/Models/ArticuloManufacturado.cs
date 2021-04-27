using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSabor.Models
{
    public class ArticuloManufacturado
    {
        public long Id { get; set; }
        [Required]
        public int TiempoEstimadoCocina { get; set; }
        public String Denominacion { get; set; }
        public double PrecioVenta { get; set; }
        public String Imagen { get; set; }
        public bool IsEnabled { get; set; }
        public long IdRubroGeneral { get; set; }
        public RubroGeneral RubroGeneral { get; set; }
        public ICollection<DetallePedido> DetallesPedidos { get; set; }
        public ICollection<DetalleFactura> DetallesFacturas { get; set; }
        public ICollection<ArticuloManufacturadoDetalle> ArticuloManufacturadoDetalles { get; set; }    //composicion


    }
}
