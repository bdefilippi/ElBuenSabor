using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSabor.Models
{
    public class ArticuloInsumo
    {
        public long Id { get; set; }
        public String Denominacion { get; set; }
        public double PrecioCompra { get; set; }
        public double PrecioVenta { get; set; }
        public double StockActual { get; set; }
        public double StockMinimo { get; set; }
        public String UnidadMedida { get; set; }
        public bool EsInsumo { get; set; }
        public bool IsEnabled { get; set; }
        public ICollection<ArticuloManufacturadoDetalle> ArticuloManufacturadoDetalles { get; set; }
        public ICollection<DetallePedido> DetallePedidos { get; set; }
        public ICollection<DetalleFactura> DetalleFacturas { get; set; }
        public long IdRubroArticulo { get; set; }
        public RubroArticulo RubroArticulo { get; set; }

    }
}
