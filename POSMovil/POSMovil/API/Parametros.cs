using System;
using System.Collections.Generic;
using System.Text;

namespace POSMovil.API
{
    public class Parametros
    {
		public int id { get; set; }
		public int nit { get; set; }
		public string nombre1 { get; set; }
		public string nombre2 { get; set; }
		public string direccion { get; set; }
		public string zona { get; set; }
		public string telefonos { get; set; }
		public string ciudad { get; set; }
		public byte[] logo { get; set; }
		public string leyenda { get; set; }
		public byte accesodeta { get; set; }
		public string nomb_prec1 { get; set; }
		public string nomb_prec2 { get; set; }
		public string nomb_prec3 { get; set; }
		public string url_facts { get; set; }
		public string email_serv { get; set; }
		public int email_port { get; set; }
		public byte email_ssl { get; set; }
		public string email_user { get; set; }
		public string email_pass { get; set; }
		public string email_send { get; set; }
		public byte sendmail { get; set; }
		public byte talcual1er { get; set; }
		public int logitud_nf { get; set; }
		public byte imprimir { get; set; }
		public byte original { get; set; }
		public byte copia_cont { get; set; }
		public byte copia_adm { get; set; }
		public byte preview { get; set; }
	}
}
