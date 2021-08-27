using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSabor.Hubs
{
    public class SignalRGroups
    {
        //Registro la cantidad de concineros que se logean para poder calcular los tiemosEstimado de los pedidos
        public int Cocineros { get; set; }
    }
}
