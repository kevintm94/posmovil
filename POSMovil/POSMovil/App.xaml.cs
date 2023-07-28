using POSMovil.API;
using POSMovil.Model;
using POSMovil.Sesion;
using POSMovil.View;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace POSMovil
{
    public partial class App : Application, ILoginManager
    {
        public static HttpClient RestClient { get; private set; }
        public static string BaseUrl { get; private set; }
        static ILoginManager loginManager;
        public static App Current;
        public static int val;

        public App()
        {
            InitializeComponent();

            App.RestClient = new HttpClient(new Dictionary<string, string> 
            {
                { "X-Api-Key", "8e929432-bb29-4967-9567-319d8e72608e" }
            });
            //App.BaseUrl = "http://api.nexxosrl.site";
            //App.BaseUrl = "http://dev.nexxosrl.site";
            App.BaseUrl = "http://test.nexxosrl.site";

            Current = this;
            var isLoggedIn = Properties.ContainsKey("IsLoggedIn") ? (bool)Properties["IsLoggedIn"] : false;
            if (isLoggedIn)
            {
                MainPage = new NavigationPage(new HomePage());
            }
            else
            {
                MainPage = new LoginPage(this);
            }

        }

        protected override void OnStart()
        {
            
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        public void ShowMainPage()
        {
            MainPage = new NavigationPage(new HomePage());
        }

        public void Logout()
        {
            Properties["IsLoggedIn"] = false;
            MainPage = new LoginPage(this);
        }
    }
}
