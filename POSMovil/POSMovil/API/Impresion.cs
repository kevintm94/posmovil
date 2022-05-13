using LinkOS.Plugin;
using LinkOS.Plugin.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace POSMovil.API
{
    public class Impresion
    {
        public Impresion()
        {

        }

        public async Task PrintBluetooth(string det)
        {
            /*****Impresora Mudanza*****/
            IConnection connection = ConnectionBuilder.Current.Build("BT:04:EE:03:17:FE:AA");
            
            /*****Impresora Limpieza*****/
            //IConnection connection = ConnectionBuilder.Current.Build("BT:04:EE:03:18:68:F8");
            System.Threading.Thread.Sleep(700);
            await Print(connection, det);
        }
        public async Task PrintBluetooth(string det, string copia)
        {
            /*****Impresora Mudanza*****/
            IConnection connection = ConnectionBuilder.Current.Build("BT:04:EE:03:17:FE:AA");

            /*****Impresora Limpieza*****/
            //IConnection connection = ConnectionBuilder.Current.Build("BT:04:EE:03:18:68:F8");
            System.Threading.Thread.Sleep(700);
            await Print(connection, det);
            System.Threading.Thread.Sleep(1500);
            await Print(connection, copia);
        }
        public async Task Print(IConnection connection, string det)
        {
            //string zpl = "^XA^POI^MNN^PW550^LL180^CF1,30,12^FO20,50^FB540,3,10,C,0^FDEMPRESEEEEA ABCD S.R.L.\\&MATRIZ\\&PLAZUEAL DEMETRIO CANELAS 999^FS^XZ^XA^LL50^XZ";
            string zpl = det;
            try
            {
                connection.Open();
                if (!CheckPrinterLanguage(connection))
                    return;
                if (!PreCheckPrinterStatus(connection))
                    return;
                if (!CheckPrinterLanguage(connection))
                    return;
                connection.Write(Encoding.UTF8.GetBytes(zpl));
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception:" + e.Message);
            }
            finally
            {
                if (connection.IsConnected)
                    connection.Close();
            }
        }

        public async Task Print(IConnection connection, string det, string copia)
        {
            string zpl = det;
            string zpl2 = copia;
            try
            {
                connection.Open();
                if (!CheckPrinterLanguage(connection))
                    return;
                if (!PreCheckPrinterStatus(connection))
                    return;
                if (!CheckPrinterLanguage(connection))
                    return;
                connection.Write(Encoding.UTF8.GetBytes(zpl));
                System.Threading.Thread.Sleep(2000);
                if (!CheckPrinterLanguage(connection))
                    return;
                if (!PreCheckPrinterStatus(connection))
                    return;
                if (!CheckPrinterLanguage(connection))
                    return;
                connection.Write(Encoding.UTF8.GetBytes(zpl2));
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception:" + e.Message);
            }
            finally
            {
                if (connection.IsConnected)
                    connection.Close();
            }
        }

        public bool CheckPrinterLanguage(IConnection connection)
        {
            //  Set the printer command languege
            connection.Write(new UTF8Encoding().GetBytes("! U1 setvar \"device.languages\" \"zpl\"\r\n"));

            byte[] response = connection.SendAndWaitForResponse(new UTF8Encoding().GetBytes("! U1 getvar \"device.languages\"\r\n"), 500, 100);

            string language = Encoding.UTF8.GetString(response, 0, response.Length);
            if (!language.Contains("zpl"))
            {
                return false;
            }
            return true;
        }

        public bool PreCheckPrinterStatus(IConnection connection)
        {
            // Check the printer status prior to printing
            IZebraPrinter printer = ZebraPrinterFactory.Current.GetInstance(connection);
            IPrinterStatus status = printer.CurrentStatus;
            if (!status.IsReadyToPrint)
            {
                System.Diagnostics.Debug.WriteLine("Unable to print. Printer is " + status.Status);
                return false;
            }
            return true;
        }
        public bool PostPrintCheckStatus(IConnection connection)
        {
            // Check the status again to verify print happened successfully
            IZebraPrinter printer = ZebraPrinterFactory.Current.GetInstance(connection);
            IPrinterStatus status = printer.CurrentStatus;
            // Wait while the printer is printing
            while ((status.NumberOfFormatsInReceiveBuffer > 0) && (status.IsReadyToPrint))
            {
                status = printer.CurrentStatus;
            }
            // verify the print didn't have errors like running out of paper
            if (!status.IsReadyToPrint)
            {
                System.Diagnostics.Debug.WriteLine("Error durring print. Printer is " + status.Status);
                return false;
            }
            return true;
        }
    }
}
