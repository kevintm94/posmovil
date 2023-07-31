using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace POSMovil.API
{
    public class Cuf
    {
        public string NitEmisor { get; set; }
        public long FechaEmisor { get; set; }
        public string Sucursal { get; set; }
        public int Modalidad { get; set; }
        public int TipoEmision { get; set; }
        public int TipoFactura { get; set; }
        public string TipoDocSector { get; set; }
        public string NumeroFactura { get; set; }
        public string PuntoVenta { get; set; }

        public Cuf()
        {

        }

        public string generarCuf(bool config)
        {
            if (NitEmisor.Length < 13)
                NitEmisor = rellenarCeros(NitEmisor, NitEmisor.Length, 13);

            if (Sucursal.Length < 4)
                Sucursal = rellenarCeros(Sucursal, Sucursal.Length, 4);

            if (TipoDocSector.Length < 2)
                TipoDocSector = rellenarCeros(TipoDocSector, TipoDocSector.Length, 2);

            if (NumeroFactura.Length < 10)
                NumeroFactura = rellenarCeros(NumeroFactura, NumeroFactura.Length, 10);

            if (PuntoVenta.Length < 4)
                PuntoVenta = rellenarCeros(PuntoVenta, PuntoVenta.Length, 4);

            string cuf = NitEmisor + FechaEmisor + Sucursal + Modalidad + TipoEmision + TipoFactura + TipoDocSector + NumeroFactura + PuntoVenta;
            cuf = cuf + calculaDigitoMod11(cuf, 1, 9, false);

            //Base 16
            BigInteger cuf2 = BigInteger.Parse(cuf);
            string vef = cuf2.ToString("X");

            return config ? vef.Substring(1) : vef;
        }

        private string rellenarCeros(string cadena, int length, int v)
        {
            for (int i = 0; i < v - length; i++)
            {
                cadena = "0" + cadena;
            }
            return cadena;
        }

        public String calculaDigitoMod11(String cadena, int numDig, int limMult, bool x10)

        {
            int mult, suma, i, n, dig;

            if (!x10) numDig = 1;

            for (n = 1; n <= numDig; n++)
            {
                suma = 0;
                mult = 2;

                for (i = cadena.Length - 1; i >= 0; i--)
                {
                    suma += (mult * int.Parse(cadena.Substring(i, 1)));
                    if (++mult > limMult) mult = 2;
                }

                if (x10)
                {
                    dig = ((suma * 10) % 11) % 10;
                }
                else
                {
                    dig = suma % 11;
                }

                if (dig == 10)
                {
                    cadena += "1";
                }

                if (dig == 11)
                {
                    cadena += "0";
                }

                if (dig < 10)
                {
                    cadena += Convert.ToString(dig);
                }

            }

            return cadena.Substring(cadena.Length - numDig, 1);

        }
    }
}
