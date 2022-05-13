using POSMovil.API;
using Shiny.BluetoothLE;
using Shiny.BluetoothLE.Central;
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
    public partial class PrintPage : ContentPage
    {
        public PrintPage()
        {
            InitializeComponent();
            BtnPrint.Clicked += BtnPrint_Clicked;
        }

        private async void BtnPrint_Clicked(object sender, EventArgs e)
        {
            //await _blueToothService.Print("", BoxTexto.Text);
            Impresion imp = new Impresion();
            await imp.PrintBluetooth("");
        }
    }
}