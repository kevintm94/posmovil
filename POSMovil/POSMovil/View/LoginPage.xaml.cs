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
                await DisplayAlert("PC-POS Móvil", "Debe tener acceso a internet para iniciar sesión", "Aceptar");
                return;
            }
            Cargador.IsRunning = true;
            var userindb = await new UserRequest(App.RestClient).Get(usuario);
            if (userindb != null)
            {
                var firstUser = userindb.ElementAt(0);
                User user = User.FromUsuario(firstUser);
                if (password == user.Password)
                {
                    if (user.Cmd_99_Acc == 1) 
                    {
                        App.Current.Properties["name"] = user.Nombre;
                        App.Current.Properties["user"] = user.Id;
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
                    await DisplayAlert("PC-POS Móvil", "La contraseña ingresada no es correcta", "Aceptar");
                    return;
                }
            }
            else 
            {
                Cargador.IsRunning = false;
                await DisplayAlert("PC-POS Móvil", "El usuario ingresado no esta registrado", "Aceptar");
                return;
            }
        }
    }
}