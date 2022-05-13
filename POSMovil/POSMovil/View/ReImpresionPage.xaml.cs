using POSMovil.API;
using POSMovil.Codigos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace POSMovil.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReImpresionPage : ContentPage
    {
        private Parametros _parametros;
        private int _actividad;
        private Factura _fact;
        private FacturaDetalle _factdet;
        private Impresion _imp = new Impresion();
        public ReImpresionPage(Parametros parametros, int actividad, Factura fact, FacturaDetalle factdet)
        {
            InitializeComponent();
            _parametros = parametros;
            _actividad = actividad;
            _fact = fact;
            _factdet = factdet;
            lblNro.Text = fact.nrofact + "";
            lblNit.Text = fact.nit + "";
            lblNombre.Text = fact.nombre;
            lblTotal.Text = fact.tot_a_pag + " Bs.";
            BtnReImp.Clicked += BtnReImp_Clicked;
        }

        private async void BtnReImp_Clicked(object sender, EventArgs e)
        {
            bool primero = false;
            BtnReImp.IsEnabled = false;
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("PC-POS Móvil", "Debe tener acceso a internet para re imprimir una factura", "Aceptar");
                return;
            }
            if (!cbOriginal.IsChecked && !cbCopiaConta.IsChecked && !cbCopiaAdmin.IsChecked)
            {
                await DisplayAlert("PC-POS Móvil", "Debe seleccionar al menos una copia para re imprimir", "Aceptar");
                return;
            }
            Cargador.IsRunning = true;
            var dosific = await new DosificacionRequest(App.RestClient).Get(_fact.nroauto);
            Dosificacion dosific1 = dosific.ElementAt(0);
            if (cbOriginal.IsChecked)
            {
                primero = true;
                string original = CuerpoFactura(dosific1.ley453, _fact, _factdet, "ORIGINAL CLIENTE");
                await _imp.PrintBluetooth(original);
            }

            if (cbCopiaConta.IsChecked)
            {
                if (primero)
                {
                    await DisplayAlert("PC-POS Móvil", "Continuar con la impresión de la copia contabilidad", "Aceptar");
                    primero = true;
                }
                string copiaC = CuerpoFactura(dosific1.ley453, _fact, _factdet, "COPIA CONTABILIDAD");
                await _imp.PrintBluetooth(copiaC);
            }

            if (cbCopiaAdmin.IsChecked)
            {
                if (primero)
                {
                    await DisplayAlert("PC-POS Móvil", "Continuar con la impresión de la copia administrativa", "Aceptar");
                    primero = true;
                }
                string copiaA = CuerpoFactura(dosific1.ley453, _fact, _factdet, "COPIA ADMINISTRATIVA");
                await _imp.PrintBluetooth(copiaA);
            }
            
            lblCargando.Text = "";
            bool fdd = await new FacturaDetalleRequest(App.RestClient).Delete(_factdet.idfact + "");
            bool fd = await new FacturaRequest(App.RestClient).Delete(_fact.idfact + "");
            //BtnReImp.IsEnabled = true;
            Cargador.IsRunning = false;
            await Navigation.PopAsync();
        }

        private string CuerpoFactura(string ley453, Factura facmae, FacturaDetalle facdet, string tipo)
        {
            Conversion c = new Conversion();
            string text = c.enletras(facmae.tot_a_pag + "");
            string nombre = facmae.nombre;
            text = text[0] + text.Substring(1).ToLower();
            text += " Bolivianos";
            string cadena = "^XA^POI^MNN^CI28^PW560^CF1,30,12";
            int filas;
            filas = (_parametros.nombre1.Length > 45) ? (_parametros.nombre1.Length / 45) + 1 : 1;
            cadena += "^LL" + (((filas + 1) * 40) + 5);
            cadena += "^FO20,50^FB540,3,10,C,0^FD" + _parametros.nombre1 + ".\\&MATRIZ^FS^XZ";
            cadena += "^XA^POI^MNN^CI28^CF1,30,12";
            filas = (_parametros.direccion.Length > 45) ? (_parametros.direccion.Length / 45) + 1 : 1;
            cadena += "^LL" + ((filas * 40) + 5);
            cadena += "^FO20,0^FB540,3,10,C,0^FD" + _parametros.direccion + "^FS^XZ";
            cadena += "^XA^POI^MNN^CI28^CF1,30,12^LL315";
            cadena += "^FO20,0^FB540,3,10,C,0^FDTELF. " + _parametros.telefonos + "\\&" + _parametros.ciudad + " - Bolivia" + "^FS";
            cadena += "^FO20,80^FB540,3,,C,0^A0N,30,25^FDFACTURA^FS";
            cadena += "^FO20,120^FB540,3,,C,0^FD" + tipo + "^FS";
            cadena += "^FO20,160^GB540,3,3^FS";
            cadena += "^FO20,180^FB540,3,,C,0^A0N,30,25^FDNIT: " + _parametros.nit + "^FS";
            cadena += "^FO20,220^FB540,3,,C,0^A0N,30,25^FDFACTURA N°: " + facmae.nrofact + "^FS";
            cadena += "^FO20,260^FB540,3,,C,0^FDAUTORIZACIÓN N°: " + facmae.nroauto + "^FS";
            cadena += "^FO20,300^GB540,3,3^FS^XZ";
            cadena += "^XA^POI^MNN^CI28^CF1,30,12";
            switch (_actividad)
            {
                case 1:
                    filas = (_parametros.actividad1.Length > 45) ? (_parametros.actividad1.Length / 45) + 1 : 1;
                    cadena += "^LL" + ((filas * 40) + 5);
                    cadena += "^FO20,0^FB540,3,10,J,0^FD" + _parametros.actividad1 + "^FS^XZ";
                    break;
                case 2:
                    filas = (_parametros.actividad2.Length > 45) ? (_parametros.actividad2.Length / 45) + 1 : 1;
                    cadena += "^LL" + ((filas * 40) + 5);
                    cadena += "^FO20,0^FB540,3,10,J,0^FD" + _parametros.actividad2 + "^FS^XZ";
                    break;
            }
            cadena += "^XA^POI^MNN^CI28^CF1,30,12^LL65";
            cadena += "^FO20,0^GB540,3,3^FS";
            string[] split = facmae.fecha.Split("-".ToCharArray());
            cadena += "^FO20,20^FB540,3,,J,0^FDFECHA: " + split[2] + "/" + split[1] + "/" + split[0] + " " + facmae.hora + "^FS";
            cadena += "^FO420,20^FB540,3,,J,0^FDUsu.:" + facmae.userid + "^FS^XZ";
            cadena += "^XA^POI^MNN^CI28^CF1,30,12";
            filas = (nombre.Length > 30) ? (nombre.Length / 30) + 1 : 1;
            cadena += "^LL" + ((filas * 40) + 5);
            cadena += "^FO20,0^FB540,3,,L,0^FDSEÑOR(ES):^FS";
            cadena += "^FO180,0^FB360,3,10,J,0^FD" + facmae.nombre + "^FS^XZ";
            cadena += "^XA^POI^MNN^CI28^CF1,30,12^LL125";
            cadena += "^FO20,0^FB540,3,,L,0^FDNIT/CI:^FS";
            cadena += "^FO180,0^FB360,3,,J,0^FD" + facmae.nit + "^FS";
            cadena += "^FO20,40^GB540,3,3^FS";
            cadena += "^FO20,60^FB390,3,,C,0^A0N,30,25^FDCONCEPTO^FS";
            cadena += "^FO430,60^FB540,3,,J,0^A0N,30,25^FDSUBTOT^FS";
            cadena += "^FO20,100^GB540,3,3^FS^XZ";
            cadena += "^XA^POI^MNN^CI28^CF1,30,12";
            filas = (facdet.Concepto.Length > 33) ? (facdet.Concepto.Length / 33) + 1 : 1;
            cadena += "^LL" + ((filas * 40) + 45);
            cadena += "^FO20,0^FB340,4,10,J,0^FD" + facdet.Concepto + "^FS";
            cadena += "^FO430,0^FB540,3,,J,0^FD" + facdet.subtotal.ToString("#.00") + "^FS^XZ";
            cadena += "^XA^POI^MNN^CI28^CF1,30,12^LL60";
            cadena += "^FO20,0^GB540,3,3^FS";
            cadena += "^FO20,20^FB390,3,,C,0^A0N,30,25^FDIMPORTE TOTAL Bs.^FS";
            cadena += "^FO430,20^FB540,3,,J,0^A0N,30,25^FD" + facmae.tot_a_pag.ToString("#.00") + "^FS^XZ";
            cadena += "^XA^POI^MNN^CI28^CF1,30,12";
            filas = (text.Length > 40) ? (text.Length / 40) + 1 : 1;
            cadena += "^LL" + ((filas * 40) + 5);
            cadena += "^FO20,0^FB540,3,,J,0^A0N,30,25^FDSON: " + text + "^FS^XZ";
            cadena += "^XA^POI^MNN^CI28^CF1,30,12^LL370";
            cadena += "^FO20,0^FB540,3,,J,0^FDCÓDIGO DE CONTROL: " + facmae.cod_ctrl + "^FS";
            string[] fechaL = facmae.fechalim.Split("-".ToCharArray());
            cadena += "^FO20,40^FB540,3,,J,0^FDFECHA LÍMITE DE EMISIÓN: " + fechaL[2] + "/" + fechaL[1] + "/" + fechaL[0] + "^FS";
            cadena += "^FO160,80^BQN,2,7^FD.." + _parametros.nit + "|" + facmae.nrofact + "|" + facmae.nroauto + "|" + facmae.fecha + "|" + Math.Round(facmae.tot_a_pag, 2) + "|" + Math.Round(facmae.tot_a_pag, 2) + "|" + facmae.cod_ctrl + "|" + facmae.nit + "|0|0|0|0^FS^XZ";
            cadena += "^XA^POI^MNN^CI28^CF1,30,12^LL350";
            cadena += "^FO20,0^FB540,3,10,J,0^FD\"ESTA FACTURA CONTRIBUYE AL DESARROLLO DEL PAIS.EL USO ILICITO DE ESTA SERA SANCIONADO DE ACUERDO A LA LEY\"^FS";
            cadena += "^FO20,120^FB540,3,10,J,0^FDLEY No. 453: " + ley453 + "^FS";
            cadena += "^FO20,240^GB540,3,3^FS";
            cadena += "^FO20,260^FB540,3,10,J,0^FD" + _parametros.SaludoFin + "^FS^XZ";
            cadena += "^XA^LL50^XZ";
            return cadena;
        }
    }
}