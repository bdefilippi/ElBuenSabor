﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSabor.Models
{
    public class Domicilio
    {
        public long Id { get; set; }
        public String Calle { get; set; }
        public int Numero { get; set; }
        public String Localidad { get; set; }
        public bool IsEnabled { get; set; }

    }
}
