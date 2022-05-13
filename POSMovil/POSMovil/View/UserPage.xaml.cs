using POSMovil.API;
using POSMovil.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace POSMovil
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserPage : ContentPage
    {
        public ObservableCollection<User> _items;
        public UserPage()
        {
            InitializeComponent();
            _items = new ObservableCollection<User>();
            ListUsers.ItemsSource = _items;
            ListUsers.IsPullToRefreshEnabled = true;
            ListUsers.Refreshing += ListUsers_Refreshing;
            ListUsers.ItemSelected += LisUsers_ItemSelected;
            ListUsers.ItemTapped += ListUsers_ItemTapped;
            BtnAdd.Clicked += BtnAdd_Clicked;
            BtnLogOut.Clicked += BtnLogOut_Clicked;
        }

        private void BtnLogOut_Clicked(object sender, EventArgs e)
        {
            App.Current.Logout();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await RefreshItems();
        }

        private async void BtnAdd_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddUserPage(TypeAction.Add), true);
        }

        private async void ListUsers_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is User usuario)
            {
                var userindb = await new UserRequest(App.RestClient).Get(usuario.Id);
                if (userindb != null)
                {
                    foreach (var item in userindb)
                    {
                        await DisplayAlert("PC-POS Móvil", item.nombre, "Aceptar");
                    }
                }
                else 
                {
                    await DisplayAlert("PC-POS Móvil", "No se ah encontrado el usuario, actualiza la página.", "Aceptar");
                }
            }
        }

        private void LisUsers_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ListUsers.SelectedItem = null;
        }

        private async void ListUsers_Refreshing(object sender, EventArgs e)
        {
            await RefreshItems();
            ListUsers.EndRefresh();
        }

        private async Task RefreshItems() 
        {
            var listusers = await new UserRequest(App.RestClient).All();
            _items.Clear();
            foreach (var item in listusers)
            {
                _items.Add(User.FromUsuario(item, Editar, Eliminar));
            }
        }

        private async void Eliminar(User user)
        {
            var userindb = await new UserRequest(App.RestClient).Get(user.Id);
            if (userindb != null)
            {
                if (await new UserRequest(App.RestClient).Delete(user.Id))
                {
                    _items.Remove(user);
                }
            }
            else 
            {
                _items.Remove(user);
            }
        }

        private async void Editar(User user)
        {
            var userindb = await new UserRequest(App.RestClient).Get(user.Id);
            if (userindb != null)
            {
                var firstUser = userindb.ElementAt(0);
                user = User.FromUsuario(firstUser);
                await Navigation.PushAsync(new AddUserPage(TypeAction.Update, user.Id, user), true);
            }
            else 
            {
                await DisplayAlert("PC-POS Móvil", "No se ah encontrado el usuario, actualiza la página.", "Aceptar");
            }
        }
    }
}