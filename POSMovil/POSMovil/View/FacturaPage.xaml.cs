using POSMovil.API;
using POSMovil.Codigos;
using POSMovil.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace POSMovil.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FacturaPage : ContentPage
    {
        private List<Cliente> _clients;
        private Cliente _cliente;
        private List<TipoDocumentoIdentidad> _documentos;
        private Parametros _parametros;
        private int _actividad;
        private Siat _siat;
        private Impresion _imp = new Impresion();
        public FacturaPage(Parametros parametros, int actividad, Siat siat)
        {
            InitializeComponent();
            _parametros = parametros;
            _actividad = actividad;
            _siat = siat;
            BtnEditarCliente.IsEnabled = false;
            BtnEditarCliente.IsVisible = false;
            BoxNit.Completed += BoxNit_Completed;
            BtnFacturar.Clicked += BtnFacturar_Clicked;
            BtnCrearCliente.Clicked += BtnCrearCliente_Clicked;
            BtnEditarCliente.Clicked += BtnEditarCliente_Clicked;
        }

        private async void BtnEditarCliente_Clicked(object sender, EventArgs e)
        {
            await getDocumentos();
            await Navigation.PushAsync(new ClientePage(cliente: _cliente, documentos: _documentos) { Title = "Editar Cliente" }, true);
        }

        private async void BtnCrearCliente_Clicked(object sender, EventArgs e)
        {
            await getDocumentos();
            await Navigation.PushAsync(new ClientePage(documentos: _documentos) { Title = "Crear Cliente" },true);
        }

        private async void BtnFacturar_Clicked(object sender, EventArgs e)
        {
            BtnFacturar.IsEnabled = false;
            var nit = BoxNit.Text ?? "";
            var nombre = lblNombre.Text ?? "";
            var concepto = BoxDetalle.Text ?? "";
            var total = BoxTotal.Text ?? "";
            var fecha = DateTime.Now;
            var usuario = App.Current.Properties["user"];

            if (string.IsNullOrEmpty(nit))
            {
                await DisplayAlert("POS Móvil", "Ingrese un documento de identidad registrado", "Aceptar");
                BtnFacturar.IsEnabled = true;
                return;
            }
            if (string.IsNullOrEmpty(nombre))
            {
                await DisplayAlert("POS Móvil", "Cliente debe tener un nombre o razon social", "Aceptar");
                BtnFacturar.IsEnabled = true;
                return;
            }
            if (string.IsNullOrEmpty(concepto))
            {
                await DisplayAlert("POS Móvil", "Ingrese el concepto o detalle de la factura", "Aceptar");
                BtnFacturar.IsEnabled = true;
                return;
            }
            if (string.IsNullOrEmpty(total))
            {
                await DisplayAlert("POS Móvil", "Ingrese el total a facturar", "Aceptar");
                BtnFacturar.IsEnabled = true;
                return;
            }
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("PC-POS Móvil", "Debe tener acceso a internet para generar una factura", "Aceptar");
                BtnFacturar.IsEnabled = true;
                return;
            }
            nombre = nombre.ToUpper();
            nombre = nombre.Trim();
            concepto = concepto.ToUpper();
            Cargador.IsVisible = true;
            Cargador.IsRunning = true;
            var dosific = await new DosificacionRequest(App.RestClient).Get(_nroauto);
            if (dosific == null)
            {
                await DisplayAlert("PC-POS Móvil", "Dosificación inválida, cierre la aplicación y vuelva a ingresar, si el error persiste contacte al administrador", "Aceptar");
                Cargador.IsVisible = false;
                Cargador.IsRunning = false;
                return;
            }
            Dosificacion dosific1 = dosific.ElementAt(0);
            DateTime fechalim = DateTime.ParseExact(dosific1.fechalim, "yyyy-MM-dd", null);
            
            int validarVigencia = DateTime.Compare(fechalim, fecha);
            if (validarVigencia < 0)
            {
                await DisplayAlert("PC-POS Móvil", "La dosificación expiró, contacte al administrador", "Aceptar");
                Cargador.IsVisible = false;
                Cargador.IsRunning = false;
                return;
            }
            
            int nrofactura = dosific1.ultima + 1;
            var count = await new CounterRequest(App.RestClient).Get();
            Counter count1 = count.ElementAt(0);
            int idfactura = count1.idfact + 1;
            CodigoControl codigo = new CodigoControl
            {
                _llavedosific = dosific1.llave,
                _nroauto = _nroauto + "",
                _nrofact = nrofactura + "",
                _nit = nit,
                _fecha = fecha.ToString("yyyyMMdd"),
                _total = total
            };

            string codigoControl = codigo.Generar();

            Factura facturaMae = new Factura
            {
                idfact = idfactura,
                fecha = fecha.ToString("yyyy-MM-dd"),
                nrofact = nrofactura,
                nroauto = _nroauto,
                estado = 'V',
                nit = long.Parse(nit),
                nombre = nombre,
                importe = Math.Round(double.Parse(total), 2),
                ice = 0.00,
                export = 0.00,
                vent_tcero = 0.00,
                subtotal = 0.00,
                descuentos = 0.00,
                impbase_df = 0.00,
                debitof = 0.00,
                cod_ctrl = codigoControl,
                actividad = _actividad,
                nro_suc = 0,
                tot_a_pag = Math.Round(double.Parse(total), 2),
                Recibido = 0.00,
                Cambio = 0.00,
                hora = fecha.ToString("HH:mm"),
                userid = usuario.ToString(),
                fechalim = dosific1.fechalim
            };

            FacturaDetalle facturaDetalle = new FacturaDetalle 
            {
                idfact = idfactura,
                nroauto = _nroauto,
                nrofact = nrofactura,
                Concepto = concepto,
                subtotal = Math.Round(double.Parse(total), 2)
            };

            if (await new FacturaRequest(App.RestClient).Add(facturaMae))
            {
                if (await new FacturaDetalleRequest(App.RestClient).Add(facturaDetalle))
                {
                    bool primero = false;
                    dosific1.ultima = nrofactura;
                    bool dos = await new DosificacionRequest(App.RestClient).Update(dosific1, _nroauto);
                    count1.idfact = idfactura;
                    bool tres = await new CounterRequest(App.RestClient).Update(count1, 1);
                    await DisplayAlert("PC-POS Móvil", "Factura registrada", "Aceptar");
                    BoxNit.Text = "0";
                    lblNombre.Text = "SIN NOMBRE";
                    BoxDetalle.Text = "";
                    BoxTotal.Text = "";
                    if (cbOriginal.IsChecked == true)
                    {
                        primero = true;
                        string cuerpoFactura = CuerpoFactura(dosific1, facturaMae, facturaDetalle, "ORIGINAL CLIENTE");
                        await _imp.PrintBluetooth(cuerpoFactura);
                    }
                    if (cbCopiaConta.IsChecked)
                    {
                        if (primero)
                        {
                            await DisplayAlert("PC-POS Móvil", "Continuar con la impresión de la copia contabilidad", "Aceptar");
                            primero = true;
                        }
                        string copiaC = CuerpoFactura(dosific1, facturaMae, facturaDetalle, "COPIA CONTABILIDAD");
                        await _imp.PrintBluetooth(copiaC);
                    }

                    if (cbCopiaAdmin.IsChecked)
                    {
                        if (primero)
                        {
                            await DisplayAlert("PC-POS Móvil", "Continuar con la impresión de la copia administrativa", "Aceptar");
                            primero = true;
                        }
                        string copiaA = CuerpoFactura(dosific1, facturaMae, facturaDetalle, "COPIA ADMINISTRATIVA"); ;
                        await _imp.PrintBluetooth(copiaA);
                    }

                }
                else 
                {
                    await DisplayAlert("PC-POS Móvil", "No se pudo registrar la factura, vuelva a intentar", "Aceptar");
                }
            }
            else
            {
                await DisplayAlert("PC-POS Móvil", "No se pudo registrar la factura, vuelva a intentar", "Aceptar");
            }
            Cargador.IsVisible = false;
            Cargador.IsRunning = false;
            BtnFacturar.IsEnabled = true;
        }

        private async void BoxNombre_Completed(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lblNombre.Text))
            {
                await DisplayAlert("PC-POS Móvil", "Ingrese un nombre o razón", "Aceptar");
                lblNombre.Focus();
                return;
            }
            if (BoxNit.Text != "0" && BoxNit.Text != "")
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await DisplayAlert("PC-POS Móvil", "Debe tener acceso a internet para registrar o editar el nombre de un cliente", "Aceptar");
                    return;
                }
                var cliente = await new ClientRequest(App.RestClient).Get(long.Parse(BoxNit.Text));
                if (cliente != null)
                {
                    if (lblNombre.Text != cliente.Nombre)
                    {
                        cliente.Nombre = lblNombre.Text;
                        if (await new ClientRequest(App.RestClient).Update(cliente, long.Parse(BoxNit.Text)))
                        {
                            await DisplayAlert("PC-POS Móvil", "Cliente actualizado", "Aceptar");
                        }
                        else
                        {
                            await DisplayAlert("PC-POS Móvil", "No se pudo actualizar el cliente, vuelva a intentar", "Aceptar");
                        }
                    }
                }
                else
                {
                    Cliente client = new Cliente
                    {
                        nit = long.Parse(BoxNit.Text),
                        Nombre = lblNombre.Text
                    };
                    if (await new ClientRequest(App.RestClient).Add(client))
                    {
                        await DisplayAlert("PC-POS Móvil", "Cliente registrado", "Aceptar");
                    }
                    else
                    {
                        await DisplayAlert("PC-POS Móvil", "No se pudo registrar el cliente, vuelva a intentar", "Aceptar");
                    }
                }
            }
            BoxDetalle.Focus();
        }

        private async void BoxNit_Completed(object sender, EventArgs e)
        {
            lblNombre.Text = "";
            BtnEditarCliente.IsVisible = false;
            BtnEditarCliente.IsEnabled = false;
            _cliente = null;
            if (BoxNit.Text == "" || BoxNit.Text == "0")
            {
                await DisplayAlert("PC-POS Móvil", "Documento identidad no puede estar vacio o ser 0", "Aceptar");
                return;
            }
            var nit = long.Parse(BoxNit.Text);
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("PC-POS Móvil", "Debe tener acceso a internet para buscar los datos del cliente", "Aceptar");
                return;
            }
            await refresh();
            foreach (var item in _clients)
            {
                if (item.nit == nit)
                {
                    lblNombre.Text = item.Nombre;
                    BoxDetalle.Focus();
                    _cliente = item;
                    BtnEditarCliente.IsVisible = true;
                    BtnEditarCliente.IsEnabled = true;
                    return;
                }
            }
            BoxNit.Focus();
            await DisplayAlert("PC-POS Móvil", "Cliente no registrado, regitre al cliente", "Aceptar");
            return;
        }

        private async Task refresh()
        {
            _clients = await new ClientRequest(App.RestClient).All();
        }

        private async Task getDocumentos()
        {
            _documentos = await new TipoDocumentoIdentidadRequest(App.RestClient).All();
        }

        private string CuerpoFactura(Dosificacion dosific, Factura facmae, FacturaDetalle facdet, string tipo) 
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
            cadena += "^FO20,120^FB540,3,,C,0^FD" + tipo +"^FS";
            cadena += "^FO20,160^GB540,3,3^FS";
            cadena += "^FO20,180^FB540,3,,C,0^A0N,30,25^FDNIT: " + _parametros.nit + "^FS";
            cadena += "^FO20,220^FB540,3,,C,0^A0N,30,25^FDFACTURA N°: " + dosific.ultima + "^FS";
            cadena += "^FO20,260^FB540,3,,C,0^FDAUTORIZACIÓN N°: " + dosific.nroauto + "^FS";
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
            string[] split = facmae.fecha.Split("-".ToCharArray());
            cadena += "^XA^POI^MNN^CI28^CF1,30,12^LL65";
            cadena += "^FO20,0^GB540,3,3^FS";
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
            cadena += "^FO20,0^FB540,3,,J,0^FDCÓDIGO DE CONTROL: " + facmae.cod_ctrl+ "^FS";
            string[] fechaL = dosific.fechalim.Split("-".ToCharArray());
            cadena += "^FO20,40^FB540,3,,J,0^FDFECHA LÍMITE DE EMISIÓN: " + fechaL[2] + "/" + fechaL[1] + "/" + fechaL[0] + "^FS";
            cadena += "^FO160,80^BQN,2,7^FD.." + _parametros.nit + "|" + dosific.ultima + "|" + dosific.nroauto + "|" + facmae.fecha + "|" + Math.Round(facmae.tot_a_pag, 2) + "|" + Math.Round(facmae.tot_a_pag, 2) + "|" + facmae.cod_ctrl + "|" + facmae.nit + "|0|0|0|0^FS^XZ";
            cadena += "^XA^POI^MNN^CI28^CF1,30,12^LL350";
            cadena += "^FO20,0^FB540,3,10,J,0^FD\"ESTA FACTURA CONTRIBUYE AL DESARROLLO DEL PAIS.EL USO ILICITO DE ESTA SERA SANCIONADO DE ACUERDO A LA LEY\"^FS";
            cadena += "^FO20,120^FB540,3,10,J,0^FDLEY No. 453: " + dosific.ley453 + "^FS";
            cadena += "^FO20,240^GB540,3,3^FS";
            cadena += "^FO20,260^FB540,3,10,J,0^FD" + _parametros.SaludoFin + "^FS^XZ";
            cadena += "^XA^LL50^XZ";
            return cadena;
        }
    }
}