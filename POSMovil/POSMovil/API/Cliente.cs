using System;
using System.Collections.Generic;
using System.Text;

namespace POSMovil.API
{
    public class Cliente
    {
        public long nit { get; set; }
        public string complement { get; set; }
        public int? cdtipodoc { get; set; }
        public string Nombre { get; set; }
        public string direccion { get; set; }
        public string telefonos { get; set; }
        public string celular_wa { get; set; }
        public string email { get; set; }
    }
}
