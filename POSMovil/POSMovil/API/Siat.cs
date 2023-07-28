using System;
using System.Collections.Generic;
using System.Text;

namespace POSMovil.API
{
    public class Siat
    {
		public int id { get; set; }
		public string docsec { get; set; }
		public int tipdocsec { get; set; }
		public string cod_sis { get; set; }
		public int ambiente { get; set; }
		public int modalidad { get; set; }
		public int tipo_emi { get; set; }
		public int tipo_fact { get; set; }
		public string token { get; set; }
		public DateTime token_fv { get; set; }
		public string cuis { get; set; }
		public DateTime cuis_fv { get; set; }
		public DateTime sinc_cat_f { get; set; }
		public byte cat01 { get; set; }
		public byte cat02 { get; set; }
		public byte cat03 { get; set; }
		public byte cat04 { get; set; }
		public byte cat05 { get; set; }
		public byte cat06 { get; set; }
		public byte cat07 { get; set; }
		public byte cat08 { get; set; }
		public byte cat09 { get; set; }
		public byte cat10 { get; set; }
		public byte cat11 { get; set; }
		public byte cat12 { get; set; }
		public byte cat13 { get; set; }
		public byte cat14 { get; set; }
		public byte cat15 { get; set; }
		public byte cat16 { get; set; }
		public byte cat17 { get; set; }
		public string cufd { get; set; }
		public string cufdcdctrl { get; set; }
		public string cufd_direc { get; set; }
		public DateTime cufd_fv { get; set; }
		public int cufd_alert { get; set; }
		public string cafc { get; set; }
		public string cafc2 { get; set; }
		public int formatoimp { get; set; }
		public string siatserver { get; set; }
		public int ptovta { get; set; }
		public int sucursal { get; set; }
	}
}
