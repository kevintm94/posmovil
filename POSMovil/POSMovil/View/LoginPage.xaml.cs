using POSMovil.API;
using POSMovil.Model;
using POSMovil.Sesion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace POSMovil
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public static string username = "";
        ILoginManager _iml = null;
        public LoginPage(ILoginManager iml)
        {
            InitializeComponent();
            _iml = iml;
            BoxUser.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeCharacter);
            BoxPassword.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeCharacter);
            BtnLogin.Clicked += BtnLogin_Clicked;
            BoxUser.Completed += (s, e) => BoxPassword.Focus();
            BoxPassword.TextChanged += BoxPassword_TextChanged;
        }

        private void BoxPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            BoxPassword.Text = BoxPassword.Text.ToUpper();
        }

        private async void BtnLogin_Clicked(object sender, EventArgs e)
        {
            var usuario = BoxUser.Text ?? "";
            var password = BoxPassword.Text ?? "";
            if (string.IsNullOrEmpty(usuario))
            {
                await DisplayAlert("PC-POS Móvil", "Ingrese un usuario", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                await DisplayAlert("PC-POS Móvil", "Ingrese una contraseña", "Aceptar");
                return;
            }
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("PC-POS Móvil", "No tiene acceso a internet, vuelva a intentar", "Aceptar");
                return;
            }
            Cargador.IsRunning = true;
            var user = await new UserRequest(App.RestClient).Get(usuario);
            if (user != null)
            {
                if (password == user.password)
                {
                    if (user.Cmd_99_Acc == 1) 
                    {
                        App.Current.Properties["name"] = user.nombre;
                        App.Current.Properties["user"] = user.id;
                        App.Current.Properties["limpieza"] = false;
                        App.Current.Properties["mudanza"] = false;
                        foreach (var permiso in user.Cmd_99)
                        {
                            if (permiso == 'L') App.Current.Properties["limpieza"] = true;
                            if (permiso == 'M') App.Current.Properties["mudanza"] = true;
                        }
                        App.Current.Properties["IsLoggedIn"] = true;
                        Cargador.IsRunning = false;
                        _iml.ShowMainPage();
                    } 
                    else 
                    {
                        Cargador.IsRunning = false;
                        await DisplayAlert("PC-POS Móvil", "Usuario no autorizado", "Aceptar");
                        return;
                    }
                }
                else 
                {
                    Cargador.IsRunning = false;
                    await DisplayAlert("PC-POS Móvil", "Contraseña erronea", "Aceptar");
                    return;
                }
            }
            else 
            {
                Cargador.IsRunning = false;
                await DisplayAlert("PC-POS Móvil", "El usuario no esta registrado", "Aceptar");
                return;
            }
        }
    }
}