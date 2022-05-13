using System;
using System.Collections.Generic;
using System.Text;

namespace POSMovil.API
{
    public class FacturaDetalle
    {
        public int idfact { get; set; }
        public long nroauto { get; set; }
        public int nrofact { get; set; }
        public string Concepto { get; set; }
        public double subtotal { get; set; }
    }
}
