using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace POSMovil.Sesion
{
    public class LoginModalPager : CarouselPage
    {
        ContentPage login;
        public LoginModalPager(ILoginManager ilm)
        {
            login = new LoginPage(ilm);
            
            this.Children.Add(login);
            MessagingCenter.Subscribe<ContentPage>(this, "Login", (sender) => 
            {
                this.SelectedItem = login;
            });
        }
    }
}
