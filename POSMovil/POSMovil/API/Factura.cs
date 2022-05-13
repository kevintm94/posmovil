using System;
using System.Collections.Generic;
using System.Text;

namespace POSMovil.API
{
    public class Factura
    {
        public int idfact { get; set; }
        public string fecha { get; set; }
        public int nrofact { get; set; }
        public long nroauto { get; set; }
        public char estado { get; set; }
        public long nit { get; set; }
        public string nombre { get; set; }
        public double importe { get; set; }
        public double ice { get; set; }
        public double export { get; set; }
        public double vent_tcero { get; set; }
        public double subtotal { get; set; }
        public double descuentos { get; set; }
        public double impbase_df { get; set; }
        public double debitof { get; set; }
        public string cod_ctrl { get; set; }
        public int actividad { get; set; }
        public int nro_suc { get; set; }
        public double tot_a_pag { get; set; }
        public double Recibido { get; set; }
        public double Cambio { get; set; }
        public string hora { get; set; }
        public string userid { get; set; }
        public string fechalim { get; set; }
    }
}
