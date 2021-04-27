using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSabor.Models
{
    public class ArticuloManufacturadoDetalle
    {
        public long Id { get; set; }
        public double Cantidad { get; set; }
        public String UnidadMedida { get; set; }
        public bool IsEnabled { get; set; }
        public long IdArticuloManufacturado { get; set; }
        public ArticuloManufacturado ArticuloManufacturado { get; set; }    //composicion
        public long IdArticuloInsumo { get; set; }
        public ArticuloInsumo ArticuloInsumo { get; set; }

    }
}
