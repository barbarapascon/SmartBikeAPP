using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Plugin.Geolocator;
using SmartBikeApp.Model;
using SmartBikeApp.Service;
using SmartBikeApp.ViewModel;
using Xamarin.Essentials;
//using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Markup;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace SmartBikeApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Main : MasterDetailPage
    {
        public Main()
        {
            InitializeComponent();
            //Lendo uma imagem base64 e atribuíndo à foto do usuário.
            /*
             * string base64Image = "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";
               byte[] Base64Stream = Convert.FromBase64String(base64Image);
               User_image.Source = ImageSource.FromStream(() => new MemoryStream(Base64Stream));
            */
            tapDestination_Tapped(new Object(), new EventArgs());          
        }

        private void NewRide_Tapped(object sender, EventArgs e)
        {
            DoAnimation(sender);
            ScannerAsync();            
            IsPresented = false;
        }

        private void tapPerfil_Tapped(object sender, EventArgs e)
        {
            DoAnimation(sender);
            Detail = new NavigationPage(new PerfilPage());
            IsPresented = false;

        }

        private void tapHistory_Tapped(object sender, EventArgs e)
        {
            DoAnimation(sender);
            Detail = new NavigationPage(new HistoricoPage());
            IsPresented = false;
        }

        private void tapDestination_Tapped(object sender, EventArgs e)
        {
            DoAnimation(sender);
            Detail = new NavigationPage(new FindBikesPage(MasterDetailHome, false));
            IsPresented = false;
        }
        private void tapAbout_Tapped(object sender, EventArgs e)
        {
            DoAnimation(sender);
            Detail = new NavigationPage(new AboutPage());
            IsPresented = false;
        }

        private async void tapLogout_Tapped(object sender, EventArgs e)
        {
            DoAnimation(sender);
            await Navigation.PopModalAsync();

            //Application.Current.MainPage = new LoginPage();
        }


        /// <summary>
        /// Animação para fazer com que os frames e os StackLayouts clicáveis, tenham animação parecida com a animação de botões.
        /// </summary>
        /// <param name="sender"></param>
        private async void DoAnimation(object sender)
        {
            const int _animationTime = 50;
            try
            {
                if (sender.GetType().Name == "StackLayout")
                {
                    var layout = (StackLayout)sender;
                    await layout.FadeTo(0.5, _animationTime);
                    await layout.FadeTo(1, _animationTime);
                }
                else if (sender.GetType().Name == "Frame")
                {
                    var layout = (Frame)sender;
                    await layout.FadeTo(0.5, _animationTime);
                    await layout.FadeTo(1, _animationTime);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Button animation error", ex.Message, "OK");
            }
        }

        private async void ScannerAsync()
        {
            ToolbarItem item = new ToolbarItem
            {
                Text = "Leitor de QR Code",
                IconImageSource = ImageSource.FromFile("qr_code.png"),
                Order = ToolbarItemOrder.Primary,
                Priority = 0
            };

            var scannerPage = new ZXingScannerPage();
            scannerPage.Title = "Leitor de QR Code";
            scannerPage.ToolbarItems.Add(item);

            await Detail.Navigation.PopToRootAsync();
            scannerPage.OnScanResult += (result) =>
            {                
                scannerPage.IsScanning = false;
                Device.BeginInvokeOnMainThread(() =>
                {
                    Detail.Navigation.PopToRootAsync();
                    if (result.Text.Contains("urn:ngsi-ld:bicicleta:"))
                    {
                        var duration = TimeSpan.FromMilliseconds(200);
                        Vibration.Vibrate(duration);
                        Detail = new NavigationPage(new BicicletaInfoPage(MasterDetailHome));
                    }
                    else
                    {
                        var duration = TimeSpan.FromMilliseconds(500);
                        Vibration.Vibrate(duration);
                        DisplayAlert("valor do QRcodeLidoInválido", result.Text, "OK");
                        return;
                    }
                });
            };
            await Detail.Navigation.PushAsync(scannerPage);
        }

    }
}