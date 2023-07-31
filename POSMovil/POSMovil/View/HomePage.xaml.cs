using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POSMovil.API;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using POSMovil.Model;

namespace POSMovil.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            BtnLimpieza.IsEnabled = false;
            BtnLimpieza.IsVisible = false;
            BtnRILimpieza.IsEnabled = false;
            BtnRILimpieza.IsVisible = false;
            BtnMudanza.IsEnabled = false;
            BtnMudanza.IsVisible = false;
            BtnRIMudanza.IsEnabled = false;
            BtnRIMudanza.IsVisible = false;
            BtnLogOut.Clicked += BtnLogOut_Clicked;
            BtnLimpieza.Clicked += BtnLimpieza_Clicked;
            BtnMudanza.Clicked += BtnMudanza_Clicked;
            BtnRILimpieza.Clicked += BtnRILimpieza_Clicked;
            BtnRIMudanza.Clicked += BtnRIMudanza_Clicked;
            //BtnTest.Clicked += BtnTest_Clicked; 
            //BtnScan.Clicked += BtnScan_Clicked;
            //BtnScan.IsEnabled = false;
            //BtnScan.IsVisible = false;
        }

        private async void BtnRIMudanza_Clicked(object sender, EventArgs e)
        {
            BtnRIMudanza.IsEnabled = false;
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("PC-POS Móvil", "Debe tener acceso a internet para re imprimir una factura", "Aceptar");
                return;
            }
            var fact = await new FacturaRequest(App.RestClient).GetRI((String)App.Current.Properties["user"], 456);
            if (fact != null)
            {
                Factura fact1 = fact.ElementAt(0);
                var factdet = await new FacturaDetalleRequest(App.RestClient).GetRI(fact1.nrofact, 456);
                if (factdet != null)
                {
                    FacturaDetalle factdet1 = factdet.ElementAt(0);
                    await Navigation.PushAsync(new ReImpresionPage(new Parametros(), 2, fact1, factdet1) { Title = "Re Imprimir Factura de Mudanza" }, true);
                }
                else
                {
                    await DisplayAlert("PC-POS Móvil", "Ocurrio un error, contacte al administrador", "Aceptar");
                }
            }
            else
            {
                await DisplayAlert("PC-POS Móvil", "No tiene permiso para reimprimir una factura, contacte al administrador", "Aceptar");
            }
            BtnRIMudanza.IsEnabled = true;
        }

        private async void BtnRILimpieza_Clicked(object sender, EventArgs e)
        {
            BtnRILimpieza.IsEnabled = false;
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("PC-POS Móvil", "Debe tener acceso a internet para re imprimir una factura", "Aceptar");
                return;
            }
            var fact = await new FacturaRequest(App.RestClient).GetRI((String)App.Current.Properties["user"], 123);
            if (fact != null)
            {
                Factura fact1 = fact.ElementAt(0);
                var factdet = await new FacturaDetalleRequest(App.RestClient).GetRI(fact1.nrofact, 123);
                if (factdet != null)
                {
                    FacturaDetalle factdet1 = factdet.ElementAt(0);
                    await Navigation.PushAsync(new ReImpresionPage(new Parametros(), 1, fact1, factdet1) { Title = "Re Imprimir Factura de Limpieza" }, true);
                }
                else
                {
                    await DisplayAlert("PC-POS Móvil", "Ocurrio un error, contacte al administrador", "Aceptar");
                }
            }
            else
            {
                await DisplayAlert("PC-POS Móvil", "No tiene permiso para reimprimir una factura, contacte al administrador", "Aceptar");
            }
            BtnRILimpieza.IsEnabled = true;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            /*if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("PC-POS Móvil", "Debe tener acceso a internet para usar la aplicación", "Aceptar");
                App.Current.Logout();
                return;
            }*/
            var user = await new UserRequest(App.RestClient).Get((String)App.Current.Properties["user"]);
            if (user.Cmd_99_Acc == 1)
            {
                App.Current.Properties["limpieza"] = false;
                App.Current.Properties["mudanza"] = false;
                foreach (var permiso in user.Cmd_99)
                {
                    if (permiso == 'L') App.Current.Properties["limpieza"] = true;
                    if (permiso == 'M') App.Current.Properties["mudanza"] = true;
                }
                var limpieza = (bool)App.Current.Properties["limpieza"];
                var mudanza = (bool)App.Current.Properties["mudanza"];
                if (limpieza)
                {
                    BtnLimpieza.IsVisible = true;
                    BtnLimpieza.IsEnabled = true;
                    BtnRILimpieza.IsVisible = true;
                    BtnRILimpieza.IsEnabled = true;
                }
                if (mudanza)
                {
                    BtnMudanza.IsVisible = true;
                    BtnMudanza.IsEnabled = true;
                    BtnRIMudanza.IsVisible = true;
                    BtnRIMudanza.IsEnabled = true;
                }
            }
            else
            {
                await DisplayAlert("PC-POS Móvil", "Usuario no autorizado", "Aceptar");
                App.Current.Logout();
                return;
            }
        }

        private async void BtnMudanza_Clicked(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("PC-POS Móvil", "Debe tener acceso a internet para facturar", "Aceptar");
                return;
            }
            Siat siat = await new SiatRequest(App.RestClient).Find(0, 1, "CV");
            Parametros parametros = await new ParametrosResponse(App.RestClient).Get(1);
            await Navigation.PushAsync(new FacturaPage(parametros, 522921, siat) { Title = "Factura de Mudanza" }, true);
        }

        private async void BtnLimpieza_Clicked(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("PC-POS Móvil", "Debe tener acceso a internet para facturar", "Aceptar");
                return;
            }
            Siat siat = await new SiatRequest(App.RestClient).Find(0, 2, "CV");
            Parametros parametros = await new ParametrosResponse(App.RestClient).Get(1);
            await Navigation.PushAsync(new FacturaPage(parametros, 810000, siat) { Title = "Factura de Limpieza" }, true);
        }

        private void BtnLogOut_Clicked(object sender, EventArgs e)
        {
            App.Current.Logout();
        }
    }
}