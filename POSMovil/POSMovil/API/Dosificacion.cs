using System;
using System.Collections.Generic;
using System.Text;

namespace POSMovil.API
{
    public class Dosificacion
    {
        public long nroauto { get; set; }
        public string llave { get; set; }
        public int ultima { get; set; }
        public int alerta { get; set; }
        public string ley453 { get; set; }
        public int actividad { get; set; }
        public int activa { get; set; }
        public string fechalim { get; set; }
        public int rlock { get; set; }
    }
}
