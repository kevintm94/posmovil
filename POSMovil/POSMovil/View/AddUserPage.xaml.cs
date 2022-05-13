using POSMovil.API;
using POSMovil.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace POSMovil
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddUserPage : ContentPage
    {
        private TypeAction _action { get; set; }
        private string _iduser { get; set; }
        private User _user { get; set; }

        public AddUserPage(TypeAction action, string iduser = null, User user = null)
        {
            InitializeComponent();
            _action = action;
            _iduser = iduser;
            _user = user;
            if (action == TypeAction.Update)
            {
                if (_iduser == null) throw new NullReferenceException("IdUser no puede ser null");
                BindingContext = user ?? throw new NullReferenceException("User no puede ser null");
            }
            BoxCmd_99_Acc.Keyboard = Keyboard.Numeric;
            BtnSave.Clicked += BtnSave_Clicked;
        }

        private async void BtnSave_Clicked(object sender, EventArgs e)
        {
            BtnSave.IsEnabled = false;
            var nombre = BoxNombre.Text ?? "";
            var usuario = BoxUsuario.Text ?? "";
            var password = BoxPassword.Text ?? "";
            var cmd_99 = BoxCmd_99.Text ?? "";
            var cmd_99_acc = BoxCmd_99_Acc.Text ?? "";

            if (string.IsNullOrEmpty(nombre)) 
            {
                await DisplayAlert("POS Móvil", "Ingrese un nombre", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(usuario))
            {
                await DisplayAlert("POS Móvil", "Ingrese un usuario", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                await DisplayAlert("POS Móvil", "Ingrese una contraseña", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(cmd_99))
            {
                await DisplayAlert("POS Móvil", "Ingrese permisos", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(cmd_99_acc))
            {
                await DisplayAlert("POS Móvil", "Ingrese accesos", "Aceptar");
                return;
            }

            UserRequest request = new UserRequest(App.RestClient);
            var user = new Usuario
            {
                id = usuario,
                nombre = nombre,
                password = password,
                Cmd_99 = cmd_99,
                Cmd_99_Acc = int.Parse(cmd_99_acc)
            };
            if (_action == TypeAction.Add)
            {
                if (await request.Add(user))
                {
                    await DisplayAlert("POS Móvil", "Se creo el usuario", "Aceptar");
                }
                else
                {
                    await DisplayAlert("POS Móvil", "No se creo el usuario", "Aceptar");
                }
            }
            else 
            {
                if (_iduser != null)
                {
                    if (await request.Update(user, _iduser))
                    {
                        await DisplayAlert("POS Móvil", "Se actualizó el usuario", "Aceptar");
                    }
                    else
                    {
                        await DisplayAlert("POS Móvil", "No se actualizó el usuario", "Aceptar");
                    }
                }
                else 
                {
                
                }
            }

            BoxNombre.Text = "";
            BoxUsuario.Text = "";
            BoxPassword.Text = "";
            BoxCmd_99.Text = "";
            BoxCmd_99_Acc.Text = "";
            BtnSave.IsEnabled = true;
        }
    }

    public enum TypeAction 
    {
        Add, Update
    }
}