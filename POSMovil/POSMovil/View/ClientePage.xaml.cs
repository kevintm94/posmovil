using POSMovil.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace POSMovil.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClientePage : ContentPage
    {
        private Cliente cliente;
        public ClientePage(Cliente cliente = null, List<TipoDocumentoIdentidad> documentos = null)
        {
            InitializeComponent();
            if ( cliente != null)
            {
                this.cliente = cliente;
            } else
            {
                this.cliente = new Cliente();
            }
            var Documentos = documentos;
            BindingContext = new { this.cliente, Documentos };
            if (cliente != null)
            { 
                var selectedItem = Documentos.FirstOrDefault(item => item.codigo == cliente.cdtipodoc);
                if (selectedItem != null) Cdtipodoc.SelectedItem = selectedItem;
            }
            btnGuardar.Clicked += BtnGuardar_Clicked;
        }

        private async void BtnGuardar_Clicked(object sender, EventArgs e)
        {
            btnGuardar.IsEnabled = false;
            if (boxNit.Text == "0")
            {
                await DisplayAlert("PC-POS Móvil", "El campo documento identidad no puede ser 0", "Aceptar");
                return;

            }
            if (boxNit.Text == "" || boxNit.Text == null)
            {
                await DisplayAlert("PC-POS Móvil", "El campo documento identidad es obligatorio", "Aceptar");
                return;

            }
            if (Cdtipodoc.SelectedIndex == -1)
            {
                await DisplayAlert("PC-POS Móvil", "El campo tipo de documento identidad es obligatorio, selecione un valor de la lista", "Aceptar");
                return;

            }
            if (boxNombre.Text == "" || boxNombre.Text == null)
            {
                await DisplayAlert("PC-POS Móvil", "El campo nombre o razon social es obligatorio", "Aceptar");
                return;

            }
            
            TipoDocumentoIdentidad selected = (TipoDocumentoIdentidad)Cdtipodoc.SelectedItem;
            cliente.cdtipodoc = selected.codigo;

            Cargador.IsVisible = true;
            Cargador.IsRunning = true;
            if (this.Title == "Crear Cliente")
            {
                if (await new ClientRequest(App.RestClient).Add(cliente))
                    await DisplayAlert("PC-POS Móvil", "Cliente registrado", "Aceptar");
                else
                    await DisplayAlert("PC-POS Móvil", "No se pudo registrar el cliente, vuelva a intentar", "Aceptar");
            }
            else
            {
                if (await new ClientRequest(App.RestClient).Update(cliente, long.Parse(boxNit.Text)))
                    await DisplayAlert("PC-POS Móvil", "Cliente actualizado", "Aceptar");
                else
                    await DisplayAlert("PC-POS Móvil", "No se pudo actualizar el cliente, vuelva a intentar", "Aceptar");
            }
            boxNit.Text = "";
            boxComplemento.Text = "";
            boxNombre.Text = "";
            boxDireccion.Text = "";
            boxCelular.Text = "";
            boxTelefono.Text = "";
            boxEmail.Text = "";
            Cargador.IsVisible = false;
            Cargador.IsRunning = false;
            btnGuardar.IsEnabled = true;
        }
    }
}