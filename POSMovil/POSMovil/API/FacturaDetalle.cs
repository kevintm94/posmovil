using System;
using System.Collections.Generic;
using System.Text;

namespace POSMovil.API
{
    public class FacturaDetalle
    {
		public int idfact { get; set; }
		public string codigo { get; set; }
		public string descripcio { get; set; }
		public decimal pu { get; set; }
		public decimal cantidad { get; set; }
		public decimal descuento { get; set; }
		public decimal subtotal { get; set; }
		public int codprodsin { get; set; }
		public string cod_caeb { get; set; }
		public int unidadsin { get; set; }
		public string descunisin { get; set; }
	}
}
