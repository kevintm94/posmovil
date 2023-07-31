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
            var producto = (Producto)cboxProductos.SelectedItem;

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
            if (producto == null)
            {
                await DisplayAlert("POS Móvil", "Seleccione un servicio de la lista", "Aceptar");
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

            int validarVigencia = DateTime.Compare(_siat.cufd_fv, fecha);
            if (validarVigencia < 0)
            {
                await DisplayAlert("PC-POS Móvil", "Código único de facturación diaria expirado, contacte al administrador", "Aceptar");
                Cargador.IsVisible = false;
                Cargador.IsRunning = false;
                return;
            }
            
            var count = await new CounterRequest(App.RestClient).Get(1);
            int idfactura = count.idfact + 1;
            int nrofactura = (_siat.ptovta == 1) ? count.nrofact_1 + 1 : count.nrofact_2 + 1;

            string fechaCuf = fecha.ToString("yyyyMMddHHmmssFFF");
            if (fechaCuf.Length == 16) fechaCuf += "0";
            if (fechaCuf.Length == 15) fechaCuf += "00";
            if (fechaCuf.Length == 14) fechaCuf += "000";

            /**GENERAR CUF*/
            #region Generando CUF
            Cuf cuf = new Cuf();
            cuf.FechaEmisor = long.Parse(fechaCuf);
            cuf.Modalidad = 2;
            cuf.NitEmisor = _parametros.nit + "";
            cuf.NumeroFactura = nrofactura + "";
            cuf.PuntoVenta = _siat.ptovta + "";
            cuf.Sucursal = _siat.sucursal + "";
            cuf.TipoDocSector = _siat.tipdocsec + "";
            cuf.TipoEmision = 2;
            cuf.TipoFactura = 1;

            string cufGenerado = cuf.generarCuf(true);
            cufGenerado += _siat.cufdcdctrl;
            #endregion

            string fechaFac = fecha.ToString("yyyy-MM-ddTHH:mm:ss.FFF");
            if (fechaFac.Length == 22) fechaFac += "0";
            if (fechaFac.Length == 21) fechaFac += "00";
            if (fechaFac.Length == 19) fechaFac += ".000";

            var leyendas = await new LeyendaRequest(App.RestClient).Find(_actividad);
            var rand = new Random();

            Factura facturaMae = new Factura
            {
                idfact = idfactura,
                fh = fecha,
                nrofact = nrofactura,
                nit = _cliente.nit + "",
                complement = _cliente.complement,
                cdtipodoc = (int)_cliente.cdtipodoc,
                codclie = _cliente.nit + "",
                nombre = nombre,
                codmetpag = 1,
                nrotarjeta = 0,
                subtotal = Math.Round(decimal.Parse(total), 2),
                descuento = 0,
                total = Math.Round(decimal.Parse(total), 2),
                gift = 0,
                montoapag = Math.Round(decimal.Parse(total), 2),
                base_cf = Math.Round(decimal.Parse(total), 2),
                debitof = 0,
                fecha_emi = fechaFac,
                cuf = cufGenerado,
                tipo_fact = 1,
                tipdocsec = _siat.tipdocsec,
                tipo_emi = 2,
                tipo_emi2 = 2,
                codmoneda = 1,
                montotmnd = Math.Round(decimal.Parse(total), 2),
                codexcep = 1,
                cafc = _siat.cafc,
                leyenda = leyendas[rand.Next(0, leyendas.Count-1)].descripcio,
                cod_recep = "",
                facstatus = "FUERA DE LINEA",
                cufd = _siat.cufd,
                cufdcdctrl = _siat.cufdcdctrl,
                pqte = 0,
                cod_anula = 0,
                cod_ev_sig = 0,
                celular_wa = _cliente.celular_wa,
                email = _cliente.email,
                cod_res = 0,
                cod_es = 0,
                desc_es = "",
                sucursal = _siat.sucursal,
                ptovta = _siat.ptovta,
                usercode = usuario.ToString(),
            };

            FacturaDetalle facturaDetalle = new FacturaDetalle 
            {
                idfact = idfactura,
                codigo = producto.codigo,
                descripcio = concepto,
                pu = Math.Round(decimal.Parse(total), 2),
                cantidad = 1,
                descuento = 0,
                subtotal = Math.Round(decimal.Parse(total), 2),
                codprodsin = producto.codprodsin,
                cod_caeb = producto.cod_caeb,
                unidadsin = producto.unidadsin,
                descunisin = "",
            };

            if (await new FacturaRequest(App.RestClient).Add(facturaMae))
            {
                if (await new FacturaDetalleRequest(App.RestClient).Add(facturaDetalle))
                {
                    bool primero = false;
                    count.idfact = idfactura;
                    if (_siat.ptovta == 1)
                        count.nrofact_1 = nrofactura;
                    else
                        count.nrofact_2 = nrofactura;
                    bool tres = await new CounterRequest(App.RestClient).Update(count, 1);
                    await DisplayAlert("PC-POS Móvil", "Factura registrada", "Aceptar");
                    BoxNit.Text = "0";
                    lblNombre.Text = "";
                    BoxDetalle.Text = "";
                    BoxTotal.Text = "";
                    if (cbOriginal.IsChecked == true)
                    {
                        primero = true;
                        string cuerpoFactura = CuerpoFactura(facturaMae, facturaDetalle, "ORIGINAL CLIENTE");
                        await _imp.PrintBluetooth(cuerpoFactura);
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

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            var Productos = await new ProductoRequest(App.RestClient).Find(_actividad);

            BindingContext = new { Productos };
        }

        private string CuerpoFactura(Factura facmae, FacturaDetalle facdet, string tipo) 
        {
            Conversion c = new Conversion();
            string text = c.enletras(facmae.total + "");
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
            cadena += "^FO20,220^FB540,3,,C,0^A0N,30,25^FDFACTURA N°: " + facmae.nrofact + "^FS";
            cadena += "^FO20,260^FB540,3,,C,0^FDAUTORIZACIÓN N°: " + facmae.cuf + "^FS";
            cadena += "^FO20,300^GB540,3,3^FS^XZ";
            cadena += "^XA^POI^MNN^CI28^CF1,30,12";
            cadena += "^XA^POI^MNN^CI28^CF1,30,12^LL65";
            cadena += "^FO20,0^GB540,3,3^FS";
            cadena += "^FO20,20^FB540,3,,J,0^FDFECHA: " + facmae.fh.ToString("dd") + "/" + facmae.fh.ToString("MM") + "/" + facmae.fh.ToString("yyyy") + " " + facmae.fh.ToString("hh:mm") + "^FS";
            cadena += "^FO420,20^FB540,3,,J,0^FDUsu.:" + facmae.usercode + "^FS^XZ";
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
            filas = (facdet.descripcio.Length > 33) ? (facdet.descripcio.Length / 33) + 1 : 1;
            cadena += "^LL" + ((filas * 40) + 45);
            cadena += "^FO20,0^FB340,4,10,J,0^FD" + facdet.descripcio + "^FS";
            cadena += "^FO430,0^FB540,3,,J,0^FD" + facdet.subtotal.ToString("#.00") + "^FS^XZ";
            cadena += "^XA^POI^MNN^CI28^CF1,30,12^LL60";
            cadena += "^FO20,0^GB540,3,3^FS";
            cadena += "^FO20,20^FB390,3,,C,0^A0N,30,25^FDIMPORTE TOTAL Bs.^FS";
            cadena += "^FO430,20^FB540,3,,J,0^A0N,30,25^FD" + facmae.total.ToString("#.00") + "^FS^XZ";
            cadena += "^XA^POI^MNN^CI28^CF1,30,12";
            filas = (text.Length > 40) ? (text.Length / 40) + 1 : 1;
            cadena += "^LL" + ((filas * 40) + 5);
            cadena += "^FO20,0^FB540,3,,J,0^A0N,30,25^FDSON: " + text + "^FS^XZ";
            cadena += "^XA^POI^MNN^CI28^CF1,30,12^LL370";
            cadena += "^FO20,40^FB540,3,,J,0^FDFECHA LÍMITE DE EMISIÓN: ^FS";
            cadena += "^FO160,80^BQN,2,7^FD.." + _parametros.nit + "|" + facmae.nrofact + "|" + facmae.cuf + "|" + facmae.fecha_emi + "|" + Math.Round(facmae.total, 2) + "|" + Math.Round(facmae.total, 2) + "|" + facmae.cuf + "|" + facmae.nit + "|0|0|0|0^FS^XZ";
            cadena += "^XA^POI^MNN^CI28^CF1,30,12^LL350";
            cadena += "^FO20,0^FB540,3,10,J,0^FD\"ESTA FACTURA CONTRIBUYE AL DESARROLLO DEL PAIS.EL USO ILICITO DE ESTA SERA SANCIONADO DE ACUERDO A LA LEY\"^FS";
            cadena += "^FO20,120^FB540,3,10,J,0^FD" + facmae.leyenda + "^FS";
            cadena += "^FO20,240^GB540,3,3^FS";
            cadena += "^FO20,260^FB540,3,10,J,0^FD SALUDO FINAL ^FS^XZ";
            cadena += "^XA^LL50^XZ";
            return cadena;
        }
    }
}