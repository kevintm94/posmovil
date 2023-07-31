using System;
using System.Collections.Generic;
using System.Text;

namespace POSMovil.API
{
    public class Factura
    {
		public int idfact { get; set; }
		public DateTime fh { get; set; }
		public int nrofact { get; set; }
		public string nit { get; set; }
		public string complement { get; set; }
		public int cdtipodoc { get; set; }
		public string codclie { get; set; }
		public string nombre { get; set; }
		public int codmetpag { get; set; }
		public long nrotarjeta { get; set; }
		public decimal subtotal { get; set; }
		public decimal descuento { get; set; }
		public decimal total { get; set; }
		public decimal gift { get; set; }
		public decimal montoapag { get; set; }
		public decimal base_cf { get; set; }
		public decimal debitof { get; set; }
		public string fecha_emi { get; set; }
		public string cuf { get; set; }
		public int tipo_fact { get; set; }
		public int tipdocsec { get; set; }
		public int tipo_emi { get; set; }
		public int tipo_emi2 { get; set; }
		public int codmoneda { get; set; }
		public decimal montotmnd { get; set; }
		public int codexcep { get; set; }
		public string cafc { get; set; }
		public string leyenda { get; set; }
		public string cod_recep { get; set; }
		public string facstatus { get; set; }
		public string cufd { get; set; }
		public string cufdcdctrl { get; set; }
		public int pqte { get; set; }
		public int cod_anula { get; set; }
		public int cod_ev_sig { get; set; }
		public string celular_wa { get; set; }
		public string email { get; set; }
		public int cod_res { get; set; }
		public int cod_es { get; set; }
		public string desc_es { get; set; }
		public int sucursal { get; set; }
		public int ptovta { get; set; }
		public string usercode { get; set; }
	}
}
