using System;
using System.Collections.Generic;
using System.Text;

namespace POSMovil.Codigos
{
    public class CodigoControl
    {
        public string _llavedosific { get; set; }
        public string _nroauto { get; set; }
        public string _nrofact { get; set; }
        public string _nit { get; set; }
        public string _fecha { get; set; }
        public string _total { get; set; }


        private int largo1, largo2, largo3, largo4, largo5;
        private string digVerhoeff, nroFactura, nit, fecha, monto, llaveCifrado, allegedRC4;
        public string Generar() 
        {
            try
            {
                string llaveDos = _llavedosific;
                string llaveDosificacion = llaveDos.Substring(0, llaveDos.Length - 2);
                llaveDosificacion = llaveDosificacion.Trim();
                generarVerhoeff();
                string cadenaCompleta = generarCadena(llaveDosificacion);
                llaveCifrado = llaveDosificacion + digVerhoeff;
                allegedRC4 = encryptMessageRC4(cadenaCompleta, llaveCifrado, true);
                int valorASCII = generarValorASCII();
                string nroBase = base64(valorASCII);
                string codigoControl = encryptMessageRC4(nroBase, llaveCifrado, false);
                return codigoControl;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private void generarVerhoeff()
        {
            long suma;
            nroFactura = _nrofact.Trim();
            nit = _nit.Trim();
            int aux = (int)Math.Round(Convert.ToDouble(double.Parse(_total.Trim())), 0, MidpointRounding.ToEven);
            string prueba = _fecha;
            fecha = prueba;
            monto = "" + aux;
            for (int i = 0; i < 2; i++)
            {
                nroFactura = nroFactura + CheckSum(nroFactura);
                nit = nit + CheckSum(nit);
                fecha = fecha + CheckSum(fecha);
                monto = monto + CheckSum(monto);
            }
            suma = long.Parse(nroFactura) + long.Parse(nit) + long.Parse(fecha) + long.Parse(monto);
            string sum = "" + suma;
            for (int j = 0; j < 5; j++)
            {
                sum = sum + CheckSum(sum);
            }
            digVerhoeff = sum.Substring(sum.Length - 5);
        }
        private int CheckSum(string number)
        {
            int[,] _multiplicationTable = {
                { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                { 1, 2, 3, 4, 0, 6, 7, 8, 9, 5 },
                { 2, 3, 4, 0, 1, 7, 8, 9, 5, 6 },
                { 3, 4, 0, 1, 2, 8, 9, 5, 6, 7 },
                { 4, 0, 1, 2, 3, 9, 5, 6, 7, 8 },
                { 5, 9, 8, 7, 6, 0, 4, 3, 2, 1 },
                { 6, 5, 9, 8, 7, 1, 0, 4, 3, 2 },
                { 7, 6, 5, 9, 8, 2, 1, 0, 4, 3 },
                { 8, 7, 6, 5, 9, 3, 2, 1, 0, 4 },
                { 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 }
            };

            int[,] _permutationTable = {
                { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                { 1, 5, 7, 6, 2, 8, 3, 0, 9, 4 },
                { 5, 8, 0, 3, 7, 9, 6, 1, 4, 2 },
                { 8, 9, 1, 6, 0, 4, 3, 5, 2, 7 },
                { 9, 4, 5, 3, 1, 2, 6, 8, 7, 0 },
                { 4, 2, 8, 6, 5, 7, 3, 9, 0, 1 },
                { 2, 7, 9, 3, 8, 0, 6, 4, 1, 5 },
                { 7, 0, 4, 6, 9, 1, 3, 2, 5, 8 }
            };

            int[] _inverseTable = { 0, 4, 3, 2, 1, 5, 6, 7, 8, 9 };

            int c = 0;
            int len = number.Length;

            for (int i = 0; i < len; ++i)
                c = _multiplicationTable[c, _permutationTable[((i + 1) % 8), number[len - i - 1] - '0']];

            return _inverseTable[c];
        }
        private string generarCadena(string llave)
        {
            string cadena1, cadena2, cadena3, cadena4, cadena5, copia, respuesta;
            copia = "" + digVerhoeff;
            largo1 = int.Parse(copia.Substring(0, 1)) + 1;
            largo2 = int.Parse(copia.Substring(1, 1)) + 1;
            largo3 = int.Parse(copia.Substring(2, 1)) + 1;
            largo4 = int.Parse(copia.Substring(3, 1)) + 1;
            largo5 = int.Parse(copia.Substring(4, 1)) + 1;
            cadena1 = llave.Substring(0, largo1);
            cadena2 = llave.Substring(largo1, largo2);
            cadena3 = llave.Substring(largo1 + largo2, largo3);
            cadena4 = llave.Substring(largo1 + largo2 + largo3, largo4);
            cadena5 = llave.Substring(largo1 + largo2 + largo3 + largo4, largo5);
            respuesta = _nroauto.Trim() + cadena1 + nroFactura + cadena2 + nit + cadena3 + fecha + cadena4 + monto + cadena5;
            return respuesta;
        }
        private string encryptMessageRC4(String message, String key, Boolean unscripted)
        {
            int[] state = new int[256];
            int x = 0;
            int y = 0;
            int index1 = 0;
            int index2 = 0;
            int nmen;
            String messageEncryption = "";

            for (int i = 0; i <= 255; i++)
            {
                state[i] = i;
            }

            for (int i = 0; i <= 255; i++)
            {
                index2 = (((int)key[index1]) + state[i] + index2) % 256;
                int aux = state[i];
                state[i] = state[index2];
                state[index2] = aux;
                index1 = (index1 + 1) % key.Length;
            }

            for (int i = 0; i < message.Length; i++)
            {
                x = (x + 1) % 256;
                y = (state[x] + y) % 256;
                int aux = state[x];
                state[x] = state[y];
                state[y] = aux;
                nmen = ((int)message[i]) ^ state[(state[x] + state[y]) % 256];
                String nmenHex = nmen.ToString("X").ToUpper();
                messageEncryption = messageEncryption + ((unscripted) ? "" : "-") + ((nmenHex.Length == 1) ? ("0" + nmenHex) : nmenHex);
            }
            return (unscripted) ? messageEncryption : messageEncryption.Substring(1, messageEncryption.Length - 1);
        }
        private int generarValorASCII()
        {
            int sp1 = 0, sp2 = 0, sp3 = 0, sp4 = 0, sp5 = 0, st;
            for (int i = 0; i < allegedRC4.Length; i = i + 5)
            {
                sp1 = sp1 + (int)allegedRC4[i];
            }
            for (int i = 1; i < allegedRC4.Length; i = i + 5)
            {
                sp2 = sp2 + (int)allegedRC4[i];
            }
            for (int i = 2; i < allegedRC4.Length; i = i + 5)
            {
                sp3 = sp3 + (int)allegedRC4[i];
            }
            for (int i = 3; i < allegedRC4.Length; i = i + 5)
            {
                sp4 = sp4 + (int)allegedRC4[i];
            }
            for (int i = 4; i < allegedRC4.Length; i = i + 5)
            {
                sp5 = sp5 + (int)allegedRC4[i];
            }
            st = sp1 + sp2 + sp3 + sp4 + sp5;
            sp1 = (sp1 * st) / largo1;
            sp2 = (sp2 * st) / largo2;
            sp3 = (sp3 * st) / largo3;
            sp4 = (sp4 * st) / largo4;
            sp5 = (sp5 * st) / largo5;
            st = sp1 + sp2 + sp3 + sp4 + sp5;
            return st;
        }
        private string base64(int nro)
        {
            char[] diccionario = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                                  'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
                                  'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
                                  'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd',
                                  'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
                                  'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
                                  'y', 'z', '+', '/' };
            int cociente = 1, resto;
            string respuesta = "";
            while (cociente > 0)
            {
                cociente = nro / 64;
                resto = nro % 64;
                respuesta = diccionario[resto] + respuesta;
                nro = cociente;
            }
            return respuesta;
        }
    }
}
